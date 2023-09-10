using System.Threading.Tasks;
using teste.Models;
using teste.Models.Anemic;

namespace teste.Infrastructure.Sevices.ExchangeRate
{
    public interface IExchangeRateService
    {
        public Task<RateConvertionResult> ProcessRateConvertion(Sale sale);
    }
}
