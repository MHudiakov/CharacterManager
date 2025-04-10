using Application.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

// For now, we set memory cache, but to make it future-proof and potentially to move it to distributed cache (Redis), we make operations async. 
public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public async Task<T?> GetCachedDataAsync<T>(string key)
    {
        memoryCache.TryGetValue(key, out T value);
        return value;
    }

    public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        memoryCache.Set(key, value, expiration);
    }

    public async Task ClearCacheAsync(string key)
    {
        memoryCache.Remove(key);
    }
}