using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace teste.Repositories.Redis
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDistributedCache _cache;

        public RedisRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetValue<T>(Guid id)
        {
            var key = id.ToString().ToLower();

            var result = await _cache.GetStringAsync(key);
            if (result == null)
                return default;

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async Task<IEnumerable<T>> GetCollection<T>(string collectionKey)
        {
            var result = await _cache.GetStringAsync(collectionKey);
            if (result == null)
                return default;

            return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
        }

        public async Task SetValue<T>(string id, T obj, string expireTime)
        {
            var key = id.ToLower();

            var newValue = JsonConvert.SerializeObject(obj);

            var expire = DateTime.Parse(expireTime).AddHours(1);

            var sholdExpiresIn = new DateTimeOffset(expire, TimeSpan.Zero);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = sholdExpiresIn,
                SlidingExpiration = TimeSpan.FromSeconds(3600)
            };

            await _cache.SetStringAsync(key, newValue, cacheOptions);
        }
    }
}
