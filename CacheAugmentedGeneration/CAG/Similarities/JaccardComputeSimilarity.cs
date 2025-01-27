using CAG.Interfaces;
using CAG.Models;

namespace CAG.Similarities;
public class JaccardComputeSimilarity<V> : IComputeSimilarity
{
    public double GetSimilarity(Vector v1, Vector v2)
    {
        var dimension1 = v1.Dimension; var dimension2 = v2.Dimension;
        if (dimension1 != dimension2)
        {
            throw new ArgumentException();
        }

        var list1 = v1.Components.Keys;
        var list2 = v2.Components.Keys;
        var intersection = list1.Intersect(list2);
        var union = list1.Union(list2);

        return intersection.Count() / (double)union.Count();
    }
}
