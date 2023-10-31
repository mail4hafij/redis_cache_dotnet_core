using Newtonsoft.Json;
using StackExchange.Redis;

namespace REST.Integration
{
    public class RedisCacheService : IRedisCacheService
    {
        private ConnectionMultiplexer _redisMultiplexer;
        private readonly IConfiguration _configuration;

        public RedisCacheService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redisMultiplexer = ConnectionMultiplexer.Connect(_configuration["RedisConnectionString"]);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _redisMultiplexer.GetDatabase();
            string? result = await db.StringGetAsync(key);
            if (!string.IsNullOrEmpty(result))
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            return await Task.FromResult<T?>(default);
        }

        public async void SetAsync<T>(string key, T obj)
        {
            var db = _redisMultiplexer.GetDatabase();
            await db.StringSetAsync(key, JsonConvert.SerializeObject(obj));
        }

        public async void RemoveAsync(string key)
        {
            var db = _redisMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
