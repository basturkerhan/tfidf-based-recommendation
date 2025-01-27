using CAG.Interfaces;
using CAG.Models;

namespace CAG.Similarities
{
    public class CosineComputeSimilarity : IComputeSimilarity
    {
        public double GetSimilarity(Vector v1, Vector v2)
        {
            var dimension1 = v1.Dimension; var dimension2 = v2.Dimension;
            if (dimension1 != dimension2)
            {
                throw new ArgumentException();
            }

            var res = 0.0;
            foreach (var item1 in v1.Components.Keys)
            {
                if (!v2.Components.ContainsKey(item1)) continue;
                res = res + v1.Components[item1] * v2.Components[item1];
            }

            return res;
        }
    }
}
