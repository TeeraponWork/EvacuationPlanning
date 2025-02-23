using EvacuationPlanning.Core.Interfaces.IRedis;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace EvacuationPlanning.Infrastructure.Cache.Redis
{
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _redisDatabase;
        private readonly IConfiguration _configuration;
        public RedisService(IConfiguration configuration)
        {
            _configuration = configuration;
            _redis = ConnectionMultiplexer.Connect(_configuration["Redis:ConnectionString"].ToString());
            _redisDatabase = _redis.GetDatabase();
        }
        public async Task<bool> SetHashAsync(string key, Dictionary<string, string> data, TimeSpan? expiry = null)
        {
            var hashEntries = data.Select(x => new HashEntry(x.Key, x.Value)).ToArray();
            await _redisDatabase.HashSetAsync(key, hashEntries);
            return true;
        }
        public async Task<bool> AddToSetAsync(string key, string value)
        {
            return await _redisDatabase.SetAddAsync(key, value);
        }
        public async Task<HashSet<string>> GetSetMemberAsync(string key)
        {
            var members = await _redisDatabase.SetMembersAsync(key);
            return members.Select(m => m.ToString()).ToHashSet();
        }
        public async Task<Dictionary<string, string>> GetHashAsync(string key)
        {
            var hashEntries = await _redisDatabase.HashGetAllAsync(key);
            return hashEntries.ToDictionary(entry => entry.Name.ToString(), entry => entry.Value.ToString());
        }
        public async Task<bool> SetJsonAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var jsonData = JsonSerializer.Serialize(value);

            return await _redisDatabase.StringSetAsync(key, jsonData, expiry);
        }
        public async Task<T?> GetJsonAsync<T>(string key)
        {
            var jsonData = await _redisDatabase.StringGetAsync(key);
            if (jsonData.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(jsonData);
        }
        public async Task<bool> DeleteAsync(string key)
        {
            var result = await _redisDatabase.KeyDeleteAsync(key);
            return result;
        }
    }
}