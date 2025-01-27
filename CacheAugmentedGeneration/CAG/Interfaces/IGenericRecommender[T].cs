using CAG.Models;

namespace CAG.Interfaces
{
    public interface IGenericRecommender<T>
        where T : Entity
    {
        void SetEntities(List<T> entities);
        List<Recommend<T>> CreateRecommendation(string query, int resultCount = 1);
    }
}
