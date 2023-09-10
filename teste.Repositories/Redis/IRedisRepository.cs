using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace teste.Repositories.Redis
{
    public interface IRedisRepository
    {
        Task SetValue<T>(string id, T obj, string expireTime);

        Task<T> GetValue<T>(Guid id);

        Task<IEnumerable<T>> GetCollection<T>(string collectionKey);
    }
}
