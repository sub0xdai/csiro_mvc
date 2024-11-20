using System;

namespace csiro_mvc.Services
{
    internal class CacheEntry<T>
    {
        public T Value { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime LastAccessed { get; set; }

        public CacheEntry(T value, TimeSpan? expiration)
        {
            Value = value;
            ExpirationTime = expiration.HasValue 
                ? DateTime.UtcNow.Add(expiration.Value)
                : DateTime.UtcNow.AddHours(1);
            LastAccessed = DateTime.UtcNow;
        }

        public bool IsExpired() => DateTime.UtcNow > ExpirationTime;
    }
}
