using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using teste.Models;
using teste.Models.Anemic;

namespace teste.Infrastructure.Sevices.ExchangeRate
{
    public class ExchangeRateService : IExchangeRateService
    {
        private static string ExchangeRateApiKey => "351b37bfc54dbe41826820d1";

        /// <summary>
        /// To convert amount in US dollar if currency code already isn't is
        /// </summary>
        /// <param name="sale">The Sale entity</param>
        /// <returns>The amount value in Dollar</returns>
        public async Task<RateConvertionResult> ProcessRateConvertion(Sale sale)
        {
            try
            {
                var conversionResult = new RateConvertionResult();
                var URLString = new Uri($@"https://v6.exchangerate-api.com/v6/{ExchangeRateApiKey}/latest/USD");
                using var webClient = new WebClient();
                string json = await webClient.DownloadStringTaskAsync(URLString);
                ApiConversionObject convesion = JsonSerializer.Deserialize<ApiConversionObject>(json);

                if (convesion.ConversionRates != null)
                { 
                    //Reflection to get related value from sale to apply conversion
                    var rateValue = convesion.ConversionRates.GetType().GetProperty(sale.TransactionCurrencyCode).GetValue(convesion.ConversionRates, null);
                    conversionResult.TimeLastUpdateUtc = rateValue == null
                            ? DateTime.Now.AddHours(1)
                            : Convert.ToDateTime(convesion.TimeNextUpdateUtc);
                    conversionResult.TransactionAmount = rateValue == null
                            ? sale.TransactionAmount
                            : Math.Round(sale.TransactionAmount / Convert.ToDouble(rateValue), 2);
                }

                return conversionResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
