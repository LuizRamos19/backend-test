using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using teste.ApiCore31.Helpers;
using teste.ApiCore31.Models;
using teste.ApiCore31.Mockupping;
using Newtonsoft.Json;
using teste.ApiCore31.Constatns;
using System.Text;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Http;

namespace teste.ApiCore31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessDataController : ControllerBase
    {
        private static readonly Snapper.Core.Snapper _snapper = DataHelper.CreateOracleConnection();
        private readonly ILogger<ProcessDataController> _logger;

        public ProcessDataController(ILogger<ProcessDataController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Generate necessary tests and artifacts for development
        /// </summary>
        /// <param name="generateSalesJsonFile">Use if need to convert Sales.txt to .jason file. This one will be generated in the same path of the .txt</param>
        /// <param name="generateSaleMockEntity">Use if need to buils a mockupping from Sale entity</param>
        /// <returns>If connected</returns>
        /// <remarks>
        /// Nothing here
        /// </remarks>
        [HttpGet("TestApi")]
        [SwaggerOperation(Summary = "Test and Build", Description = "Generate necessary tests and artifacts for development")]
        [SwaggerResponse(200, "Everything works well")]
        [SwaggerResponse(400, "BAD REQUEST")]
        [SwaggerResponse(500, "SERVER ERROR")]
        public async Task<IActionResult> Get([FromQuery]bool generateSalesJsonFile = false, bool generateSaleMockEntity = false)
        {
            try
            {
                var currentTime = DateTime.Now;
                var testeResult = new StringBuilder();
                testeResult.AppendLine($"{currentTime:HH:mm:ss} -> Start API test");
                #region-- MOC testing for rate --
                if (generateSaleMockEntity)
                {
                    var sale = Mock.MockSaleEntity;
                    testeResult.AppendLine($"{(DateTime.Now-currentTime).TotalSeconds:00 secs taken} -> Sale entity mock generated.");
                    if (sale.TransactionCurrencyCode.ToUpper() != "USD")
                    {
                        var currentTransactionAmount = sale.TransactionAmount.ToString();
                        sale = await ConvertToDollar(sale);
                        testeResult.AppendLine($"{(DateTime.Now-currentTime).TotalSeconds:00 secs taken} -> Transaction Amount converted from {sale.TransactionCurrencyCode} to USD: {currentTransactionAmount} to {sale.TransactionAmount}.");
                    }
                    else {
                        testeResult.AppendLine($"{(DateTime.Now-currentTime).TotalSeconds:00 secs taken} -> No need to Exchange Transaction Amount.");
                    }
                }
                #endregion

                #region-- MOC to create json file from Sales.txt --
                if (generateSalesJsonFile) 
                {
                    await FileHelper.SalesListToJsonFile();
                    testeResult.AppendLine($"{(DateTime.Now-currentTime).TotalSeconds:00 secs taken} -> Sales.json file successfully created.");
                }
                #endregion

                _snapper.CreateCommand("SELECT sysdate FROM dual", null, CommandType.Text);
                var tblSales = await _snapper.Command.ExecuteTableAsync();
                var count = tblSales.Rows.Count;
                testeResult.AppendLine($"{(DateTime.Now-currentTime).TotalSeconds:00 secs taken} -> Success on connect to Oracle.");
                return StatusCode(200,testeResult.ToString()); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Receive some data to process in queue
        /// </summary>
        /// <returns>Não sei</returns>
        [HttpPost]
        public async Task Post([FromBody] Sale sale)
        {
            await CreateTopicAsync("","ss");
            _snapper.CreateCommand("select * from sales", null, CommandType.Text);
            var tblSales = await _snapper.Command.ExecuteTableAsync();
            var count = tblSales.Rows.Count;
        }
    
        static async Task CreateTopicAsync(string bootstrapServers, string topicName) {
            try
            {
                using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();
                await adminClient.CreateTopicsAsync(new TopicSpecification[] {
                    new TopicSpecification { Name = "myTopicName", ReplicationFactor = 1, NumPartitions = 1 } });
            }
            catch (CreateTopicsException e) 
            {
                Console.WriteLine($"An error occured creating topic {e.Results[0].Topic}: {e.Results[0].Error.Reason}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured creating topic {ex.Message}");
            }
        }

        /// <summary>
        /// To convert amount in US dollar if currency code already isn't is
        /// </summary>
        /// <param name="sale">The Sale entity</param>
        /// <returns>The amount value in Dollar</returns>
        static async Task<Sale> ConvertToDollar(Sale sale)
        {
            try
            {
                var URLString = new Uri($@"https://v6.exchangerate-api.com/v6/{Credentials.ExchangeApiKey}/latest/USD");
                using var webClient = new System.Net.WebClient();
                var json = await webClient.DownloadStringTaskAsync(URLString);
                ApiConversionObject convesion = JsonConvert.DeserializeObject<ApiConversionObject>(json);

                //Reflection to get related value from sale to apply conversion
                var rateValue = convesion.ConversionRates.GetType().GetProperty(sale.TransactionCurrencyCode).GetValue(convesion.ConversionRates, null);
                if (rateValue == null) return sale;
                sale.TransactionAmount /= Convert.ToDouble(rateValue);
                return sale;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
    }
}
