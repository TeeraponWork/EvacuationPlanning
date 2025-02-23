namespace EvacuationPlanning.Core.Interfaces.IRedis
{
    public interface IRedisService
    {
        Task<bool> SetHashAsync(string key, Dictionary<string, string> data, TimeSpan? expiry = null);
        Task<bool> AddToSetAsync(string key, string value);
        Task<HashSet<string>> GetSetMemberAsync(string key);
        Task<Dictionary<string, string>> GetHashAsync(string key);
        Task<bool> SetJsonAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<T?> GetJsonAsync<T>(string key);
        Task<bool> DeleteAsync(string key);
    }
}
