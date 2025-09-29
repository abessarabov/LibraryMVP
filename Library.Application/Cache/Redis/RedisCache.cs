using Library.Domain.Cache;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Library.Server.Cache.Redis
{
    public class RedisCache : ICache
    {
        private readonly IDatabase _db;

        public RedisCache(IOptions<RedisOptions> options)
        {
            var redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
            _db = redis.GetDatabase();
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            using var ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize(ms, value);
            await _db.StringSetAsync(key, ms.ToArray(), expiry);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var bytes = await _db.StringGetAsync(key);
            if (bytes.IsNullOrEmpty) return default;

            using var ms = new MemoryStream(bytes);
            return ProtoBuf.Serializer.Deserialize<T>(ms);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}
