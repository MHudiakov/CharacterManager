namespace Application.Contracts;

public interface ICacheService
{
    Task<T?> GetCachedDataAsync<T>(string key);
    
    Task SetCacheAsync<T>(string key, T value, TimeSpan expiration);
    
    Task ClearCacheAsync();
}