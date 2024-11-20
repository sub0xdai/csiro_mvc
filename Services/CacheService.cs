using System.Collections.Concurrent;
using Serilog;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Services
{
    public class CacheService : ICacheService
    {
        private readonly ConcurrentDictionary<string, object> _cache;
        private readonly ILogger _logger;

        public CacheService()
        {
            _cache = new ConcurrentDictionary<string, object>();
            _logger = Log.ForContext<CacheService>();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    var typedEntry = entry as CacheEntry<T>;
                    if (typedEntry != null && !typedEntry.IsExpired())
                    {
                        typedEntry.LastAccessed = DateTime.UtcNow;
                        return typedEntry.Value;
                    }
                    await RemoveAsync(key);
                }
                return default;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving value for key: {Key}", key);
                throw;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            try
            {
                var entry = new CacheEntry<T>(value, expirationTime);
                _cache.AddOrUpdate(key, entry, (_, _) => entry);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error setting value for key: {Key}", key);
                throw;
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                _cache.TryRemove(key, out _);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error removing key: {Key}", key);
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    if (entry is CacheEntry<object> typedEntry && !typedEntry.IsExpired())
                    {
                        return true;
                    }
                    await RemoveAsync(key);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error checking existence for key: {Key}", key);
                throw;
            }
        }

        public async Task ClearAsync()
        {
            try
            {
                _cache.Clear();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error clearing cache");
                throw;
            }
        }
    }
}
