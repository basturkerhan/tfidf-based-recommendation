namespace CAG.Models
{
    public class Recommend<T>
    {
        public T? Entity { get; set; }
        public double Similarity { get; set; }
    }
}
