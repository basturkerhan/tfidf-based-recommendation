namespace CAG.Models
{
    public class Scored
    {
        public Scored() { }

        public string? Id { get; set; }
        public Dictionary<string, int> Terms { get; set; } = [];
        public Dictionary<string, double> TermsScored { get; set; } = [];

        public void AddTerm(string term)
        {
            if (Terms.TryGetValue(term, out int value))
            {
                Terms[term] = value + 1;
            }
            else
            {
                Terms.Add(term, 1);
            }
        }

        public double ComputeTermFrequency(string term)
        {
            if (!Terms.TryGetValue(term, out int value)) return 0.0;
            return Math.Log10(1 + value);
        }
    }
}
