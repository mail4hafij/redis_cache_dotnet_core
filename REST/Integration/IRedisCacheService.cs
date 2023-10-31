namespace REST.Integration
{
    public interface IRedisCacheService
    {
        Task<T?> GetAsync<T>(string key);
        void SetAsync<T>(string key, T obj);
        void RemoveAsync(string key);
    }
}
