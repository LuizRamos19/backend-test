using System.Threading.Tasks;

namespace teste.ApiCore31.Infrastructure.Caching
{
    public interface ICachingService
    {
        Task SetAsync(string key, string value);
        Task<string> GetAsync(string key);
    }
}
