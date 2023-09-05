using System;
using System.Threading.Tasks;

namespace teste.ApiCore31.Infrastructure.Caching
{
    public interface ICachingService
    {
        Task SetAsync(string key, string value, DateTime exppires);
        Task<string> GetAsync(string key);
    }
}
