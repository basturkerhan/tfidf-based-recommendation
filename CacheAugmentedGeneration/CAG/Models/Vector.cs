namespace CAG.Models
{
    public class Vector
    {
        public string? Id { get; set; }
        public long Dimension { get; set; }
        public Dictionary<long, double> Components { get; set; } = [];

        public void Set(long id, double value)
        {
            Components[id] = value;
        }
    }
}
