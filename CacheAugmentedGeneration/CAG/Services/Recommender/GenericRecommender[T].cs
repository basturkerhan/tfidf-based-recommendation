using CAG.Interfaces;
using CAG.Models;
using CAG.Utils.Helpers;

namespace CAG.Services.Recommender
{
    public class GenericRecommender<T>(IComputeSimilarity similarity) : IGenericRecommender<T> where T : Entity
    {
        private List<T> _entities = [];
        private readonly IComputeSimilarity _similarity = similarity;

        public void SetEntities(List<T> entities)
        {
            _entities = entities;
        }

        public List<Recommend<T>> CreateRecommendation(string query, int resultCount = 1)
        {
            query = StringHelper.NormalizeString(query);
            var processed = ProcessEntities();
            var idfs = ComputeIdf(processed);
            ComputeTfIdf(processed, idfs);
            var vectors = CreateVectors(processed, idfs);
            var queryVector = CreateQueryVector(query, idfs);

            return CalculateSimilarities(vectors, queryVector, resultCount);
        }

        private List<Scored> ProcessEntities()
        {
            List<Scored> processed = [];
            var corpora = new Dictionary<string, long>();
            foreach (var entity in _entities)
            {
                var combinedText = StringHelper.NormalizeString(entity.GetCombinedText());
                if (string.IsNullOrWhiteSpace(combinedText)) continue;

                var words = StringHelper.SplitWord(combinedText);
                foreach (var word in words.Distinct())
                {
                    corpora[word] = corpora.TryGetValue(word, out var value) ? value + 1 : 1;
                }

                Scored scored = new()
                {
                    Id = entity.Id
                };
                foreach (var word in words)
                {
                    scored.AddTerm(word);
                }

                processed.Add(scored);
            }

            return processed;
        }

        private static Dictionary<string, double> ComputeIdf(List<Scored> dialogs)
        {
            var corpora = dialogs
                .SelectMany(d => d.Terms.Keys)
                .GroupBy(word => word)
                .ToDictionary(g => g.Key, g => g.Count());
            var D = corpora.Keys.Count;
            return corpora.ToDictionary(word => word.Key, word => Math.Log10((double)D / word.Value));
        }

        private static void ComputeTfIdf(List<Scored> scoreds, Dictionary<string, double> idfs)
        {
            foreach (var scored in scoreds)
            {
                foreach (var term in scored.Terms.Keys)
                {
                    scored.TermsScored[term] = scored.ComputeTermFrequency(term) * idfs[term];
                }
            }
        }

        private static List<Vector> CreateVectors(List<Scored> scoreds, Dictionary<string, double> idfs)
        {
            var dimensions = idfs.Keys.OrderBy(t => t).ToList();
            var dimension = dimensions.Count;
            var map = dimensions.Select((word, index) => new { word, index })
                                .ToDictionary(x => x.word, x => (long)x.index);
            var vectors = new List<Vector>();
            foreach (var scored in scoreds)
            {
                Vector vector = new()
                {
                    Id = scored.Id,
                    Dimension = dimension,
                };
                foreach (var term in scored.TermsScored.Keys)
                {
                    if (map.TryGetValue(term, out var position))
                    {
                        vector.Set(position, scored.TermsScored[term]);
                    }
                }
                vectors.Add(vector);
            }

            return vectors;
        }

        private static Vector CreateQueryVector(string query, Dictionary<string, double> idfs)
        {
            var queryWords = StringHelper.SplitWord(query);
            var queryTermFrequencies = queryWords
                .GroupBy(word => word)
                .ToDictionary(g => g.Key, g => g.Count());
            var queryTermsScored = queryTermFrequencies
                .ToDictionary(
                    term => term.Key,
                    term => Math.Log10(1 + term.Value) * (idfs.TryGetValue(term.Key, out double value) ? value : 0));
            var dimensions = idfs.Keys.OrderBy(t => t).ToList();
            var dimension = dimensions.Count;
            var map = dimensions.Select((word, index) => new { word, index })
                                .ToDictionary(x => x.word, x => (long)x.index);
            Vector queryVector = new()
            {
                Id = "query",
                Dimension = dimension,
            };
            foreach (var term in queryTermsScored.Keys)
            {
                if (map.TryGetValue(term, out var position))
                {
                    queryVector.Set(position, queryTermsScored[term]);
                }
            }

            return queryVector;
        }

        private List<Recommend<T>> CalculateSimilarities(
            List<Vector> vectors, Vector queryVector, int count)
        {
            return vectors
                .Select(vector => new Recommend<T>
                {
                    Entity = _entities.First(s => s.Id == vector.Id),
                    Similarity = _similarity.GetSimilarity(queryVector, vector)
                })
                .OrderByDescending(x => x.Similarity)
                .Take(count)
                .ToList();
        }


    }
}
