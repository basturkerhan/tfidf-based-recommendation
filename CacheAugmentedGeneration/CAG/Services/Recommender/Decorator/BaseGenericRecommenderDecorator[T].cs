using CAG.Interfaces;
using CAG.Models;

namespace CAG.Services.Recommender.Decorator
{
    public class BaseGenericRecommenderDecorator<T>(IGenericRecommender<T> genericRecommender) : IGenericRecommender<T> where T : Entity
    {
        public readonly IGenericRecommender<T> _genericRecommender = genericRecommender;

        public virtual List<Recommend<T>> CreateRecommendation(string query, int resultCount = 1)
        {
            return _genericRecommender.CreateRecommendation(query, resultCount);
        }

        public virtual void SetEntities(List<T> entities)
        {
            _genericRecommender.SetEntities(entities);
        }
    }
}
