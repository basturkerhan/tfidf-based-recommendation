using CAG.Interfaces;
using CAG.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CAG.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly ConnectionMultiplexer _redis;
        public RedisCacheService(IOptions<RedisSettings> redisSettings)
        {
            var configString = $"{redisSettings.Value.Host}:{redisSettings.Value.Port}";
            _redis = ConnectionMultiplexer.Connect(configString);
        }

        public IDatabase GetDatabase(int db = 0)
        {
            return _redis.GetDatabase(db);
        }

    }
}
