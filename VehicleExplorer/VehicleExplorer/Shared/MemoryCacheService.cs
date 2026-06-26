using Microsoft.Extensions.Caching.Memory;

namespace VehicleExplorer.Web.Shared
{
    public sealed class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<MemoryCacheService> _logger;

        public MemoryCacheService(IMemoryCache memoryCache,ILogger<MemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<T> GetOrCreateAsync<T>(string key,Func<Task<T>> callback,TimeSpan expiration)
        {
            try
            {
                if (_memoryCache.TryGetValue(key, out T? cachedValue) &&
                    cachedValue is not null)
                {
                    return cachedValue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to get cached value due to exception");
            }

            var value = await callback();

            try
            {
                _memoryCache.Set(key, value, expiration);
            }
            catch (Exception ex)
            {
                 _logger.LogWarning(ex, "Failed to set cache key {CacheKey}", key);
            }

            return value;
        }
    }
}
