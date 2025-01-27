using StackExchange.Redis;

namespace CAG.Interfaces
{
    public interface ICacheService
    {
        IDatabase GetDatabase(int db = 0);
    }
}
