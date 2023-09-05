using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace teste.ApiCore31.Infrastructure.Caching
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;
        private static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                SlidingExpiration = TimeSpan.FromSeconds(1200),
            };
        }

        public async Task<string> GetAsync(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task SetAsync(string key, string value, DateTime exppires)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddHours(1), TimeSpan.Zero),
                SlidingExpiration = TimeSpan.FromSeconds(3600)
            };
            await _cache.SetStringAsync(key, value, cacheOptions);
        }
    }
}
