using CAG.Models;

namespace CAG.Interfaces
{
    public interface IComputeSimilarity
    {
        double GetSimilarity(Vector v1, Vector v2);
    }
}
