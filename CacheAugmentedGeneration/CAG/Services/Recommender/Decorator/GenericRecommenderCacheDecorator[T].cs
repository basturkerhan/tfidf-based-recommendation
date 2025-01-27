using CAG.Interfaces;
using CAG.Models;
using CAG.Utils.Helpers;
using StackExchange.Redis;
using System.Text.Json;

namespace CAG.Services.Recommender.Decorator
{
    public class GenericRecommenderCacheDecorator<T> : BaseGenericRecommenderDecorator<T> where T : Entity
    {
        private readonly ICacheService _cacheService;
        private readonly IDatabase _db;
        private readonly string _hashKey;
        public GenericRecommenderCacheDecorator(IGenericRecommender<T> genericRecommender, ICacheService cacheService) : base(genericRecommender)
        {
            _cacheService = cacheService;
            _db = _cacheService.GetDatabase();
            _hashKey = GenerateHashKey();
        }

        public override List<Recommend<T>> CreateRecommendation(string query, int resultCount = 1)
        {
            query = StringHelper.NormalizeString(query);
            if (_db.KeyExists(_hashKey))
            {
                RedisValue redisValue = _db.HashGet(_hashKey, query);
                if (redisValue.HasValue)
                {
                    return JsonSerializer.Deserialize<List<Recommend<T>>>(redisValue!)!;
                }
            }

            return LoadRecommendToCache(query, resultCount);
        }

        private List<Recommend<T>> LoadRecommendToCache(string query, int resultCount)
        {
            List<Recommend<T>> recommends = base.CreateRecommendation(query, resultCount);
            _db.HashSet(_hashKey, query, JsonSerializer.Serialize(recommends));

            return recommends;
        }

        private static string GenerateHashKey() => StringHelper.NormalizeString($"recommender_{typeof(T).FullName}");
    }
}
