using System.Collections.Concurrent;
using Serilog;
using Microsoft.Extensions.Caching.Memory;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Services
{
    public class CustomCacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, object> _cache;
        private readonly ILogger _logger;
        private readonly Timer _cleanupTimer;

        public CustomCacheService()
        {
            _cache = new ConcurrentDictionary<string, object>();
            _logger = Log.ForContext<CustomCacheService>();
            
            // Run cleanup every 5 minutes
            _cleanupTimer = new Timer(CleanupExpiredEntries, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out var existingEntry))
            {
                var entry = existingEntry as CacheEntry<T>;
                if (entry != null && !entry.IsExpired())
                {
                    entry.LastAccessed = DateTime.UtcNow;
                    _logger.Debug("Cache hit for key: {Key}", key);
                    return entry.Value;
                }
                await RemoveAsync(key);
            }
            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            var entry = new CacheEntry<T>(value, expirationTime);
            _cache.AddOrUpdate(key, entry, (_, _) => entry);
            await Task.CompletedTask;
        }

        public async Task RemoveAsync(string key)
        {
            _cache.TryRemove(key, out _);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            if (_cache.TryGetValue(key, out var existingEntry))
            {
                if (existingEntry is CacheEntry<object> entry && !entry.IsExpired())
                {
                    return true;
                }
                await RemoveAsync(key);
            }
            return false;
        }

        public async Task ClearAsync()
        {
            _cache.Clear();
            await Task.CompletedTask;
        }

        private void CleanupExpiredEntries(object? state)
        {
            var expiredKeys = _cache
                .Where(kvp => kvp.Value == null || (kvp.Value as IMemoryCache)?.Get(kvp.Key) == null)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in expiredKeys)
            {
                _cache.TryRemove(key, out _);
                _logger.Debug("Removed expired cache entry for key: {Key}", key);
            }
        }
    }
}
