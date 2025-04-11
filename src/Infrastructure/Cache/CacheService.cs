using Application.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Cache;

// For now, we set memory cache, but to make it future-proof and potentially to move it to distributed cache (Redis), we make operations async. 
public class CacheService(MemoryCache memoryCache) : ICacheService
{
    public Task<T?> GetCachedDataAsync<T>(string key)
    {
        memoryCache.TryGetValue(key, out T? value);
        return Task.FromResult(value);
    }

    public Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
    {
        memoryCache.Set(key, value, expiration);
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync()
    {
        memoryCache.Clear();
        return Task.CompletedTask;
    }
}