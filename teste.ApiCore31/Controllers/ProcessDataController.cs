using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Confluent.Kafka.Admin;
using teste.Models;
using teste.ApiCore31.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Net;
using teste.ApiCore31.Constatns;
using teste.ApiCore31.Infrastructure.Caching;
using teste.ApiCore31.Infrastructure.DataBase;
using System.Data;
using Snapper.Core.DataBase;
using System.Text.Json;
using StackExchange.Redis;
using Confluent.Kafka;
using Swashbuckle.AspNetCore.Annotations;

namespace teste.ApiCore31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessDataController : ControllerBase
    {
        
        private readonly ILogger<ProcessDataController> _logger;
        private readonly IKafkaMessageRepository _kafkaMessageRepository;
        private readonly ICachingService _cachingService;
        private readonly ISnapperDataBase _snapperDataBase;
        private CancellationTokenSource _cts;

        public ProcessDataController(ILogger<ProcessDataController> logger, 
            IKafkaMessageRepository kafkaMessageRepository, 
            ICachingService cachingService,
            ISnapperDataBase snapperDataBase)
        {
            _logger = logger;
            _kafkaMessageRepository = kafkaMessageRepository;
            _cachingService = cachingService;
            _snapperDataBase = snapperDataBase;
        }

        /// <summary>
        /// Receive some data to process in queue
        /// </summary>
        /// <param name="sales">The list of Sale entity to be processed</param>
        /// <returns>Não sei</returns>
        /// <remarks>
        /// Nothing herere
        /// </remarks>
        [HttpPost]
        [SwaggerOperation(Summary = "Processa sale data", Description = "Process sale record to kafka queue")]
        [SwaggerResponse(200, "Everything works well")]
        [SwaggerResponse(400, "BAD REQUEST")]
        [SwaggerResponse(500, "SERVER ERROR")]
        public async Task<IActionResult> Post([FromBody] List<Sale> sales)
        {
            Sale pickedSale = new Sale();
            var processResult = new StringBuilder();

            try
            {
                _cts = new CancellationTokenSource();
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        foreach (var sale in sales)
                        {
                            pickedSale = sale;
                            _logger.LogInformation($"Api received {sales.Count} record to process");

                            #region-- Kafka --
                            processResult.AppendLine(await CreateTopicAsync(sale));
                            #endregion

                            #region-- Redis --
                            processResult.AppendLine(await CreateRedisRecord(sale));
                            #endregion
                        }
                        _cts.Cancel();
                        _logger.LogInformation($"Api just finishes {sales.Count} records processed");
                    }
                    catch (CreateTopicsException e)
                    {
                        _logger.LogError($"Sale unprocessed by TopicsException: {e.Error}\nTopic: {e.Results[0].Topic} \nReason: {e.Results[0].Error.Reason}", pickedSale);
                        continue;
                    }
                }
                return StatusCode(200, processResult.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{ex.Message}\n{processResult}");
                return StatusCode(500, $"{ex.Message}\n{processResult}");
            }
        }

        /// <summary>
        /// Create Kafka topic
        /// </summary>
        /// <param name="sale">List of Sales entities to process</param>
        /// <returns>Process result log</returns>
        async Task<string> CreateTopicAsync(Sale sale) 
        {
            var currentTime = DateTime.Now;
            var processResult = new StringBuilder();
            processResult.AppendLine($"Record process start at: {currentTime:yyyy-MM-dd HH:mm:ss}");
            try
            {
                try
                {
                    var status = await _kafkaMessageRepository.SendMessageAsync(sale);
                    if (status == PersistenceStatus.Persisted)
                    {
                        processResult.AppendLine($"{(DateTime.Now - currentTime).TotalSeconds:In 00 secs taken} has been queued up.");
                        _logger.LogInformation($"Record {sale.AccountID} processed with status:{status}", sale);
                    }
                    processResult.AppendLine($"Record processing finished at: {currentTime:yyyy-MM-dd HH:mm:ss} with {(DateTime.Now - currentTime).TotalSeconds: secounds taken}");
                    _logger.LogInformation($"Record processing finished", processResult.ToString());
                }
                catch (CreateTopicsException e)
                {
                    var logMessage = $"Record unprocessed by TopicsException: {e.Error}\nTopic: {e.Results[0].Topic} \nReason: {e.Results[0].Error.Reason}";
                    processResult.AppendLine(logMessage);
                    _logger.LogError(logMessage, sale);
                }
                return processResult.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Record unprocessed by Exception: {ex.Message}", sale);
                throw;
            }
        }

        /// <summary>
        /// Create record on the Redis
        /// </summary>
        /// <param name="sale">Sale entitie</param>
        /// <returns>Process result log</returns>
        async Task<string> CreateRedisRecord(Sale sale)
        {
            try
            {
                var processResult = new StringBuilder();
                string logMessage;

                var todoCache = await _cachingService.GetAsync(sale.Id.ToString());
                Sale saleToDo = null;
                if (!string.IsNullOrWhiteSpace(todoCache))
                {
                    saleToDo = JsonSerializer.Deserialize<Sale>(todoCache);
                    logMessage = $"Record {sale.AccountID} already in Redis cache";
                    processResult.AppendLine(logMessage);
                    _logger.LogInformation(logMessage, saleToDo);
                    return processResult.ToString();
                }

                var oracle = _snapperDataBase.CreateOracleConnection();
                #region-- Chamada de procedure da erro quando chama base no Container --
                //PLS-00306: wrong number or types of arguments in call to 'SAVE_SALE'
                //oracle.CreateCommand(
                //    EnumExtension<OracleProcedure>.GetDisplayValue(OracleProcedure.GetSale),
                //    new List<Parameter>
                //    {
                //        new Parameter
                //        {
                //            Name = "P_AccountID",
                //            Value = sale.AccountID,
                //            ParameterDirection = ParameterDirection.Input,
                //            ParameterDbType = ParameterDbType.Varchar2
                //        },
                //        new Parameter
                //        {
                //            Name = "O_Result",
                //            Value = null,
                //            ParameterDirection = ParameterDirection.Output,
                //            ParameterDbType = ParameterDbType.RefCursor
                //        }
                //    },
                //    CommandType.StoredProcedure
                //);
                #endregion
                //Então vai texto direto mesmo.
                oracle.CreateCommand(
                    "SELECT count(*) Existe FROM sales s WHERE s.ACCOUNT_ID = (p)AccountId",
                    new List<Parameter>
                    {
                        new Parameter
                        {
                            Name = "AccountId",
                            Value = sale.AccountID,
                            ParameterDirection = ParameterDirection.Input,
                            ParameterDbType = ParameterDbType.Varchar2
                        }
                    },
                    CommandType.Text
                );
                var convesion  = new RateConvertionResult
                {
                    TimeLastUpdateUtc = DateTime.Now.AddHours(1),
                    TransactionAmount = sale.TransactionAmount
                };
                var tb = oracle.Command.ExecuteTable();
                if (Convert.ToInt64(tb.Rows[0]["Existe"]) == 0)
                {
                    //Convert Transaction Amount if not in USD
                    if (sale.TransactionCurrencyCode != "USD")
                    {
                        var currentTransactoinAmount = sale.TransactionAmount;
                        convesion = await ConvertToDollar(sale);
                        logMessage = $"Record {sale.AccountID} has Transaction Amount converted from {sale.TransactionCurrencyCode}: {currentTransactoinAmount} to USD: {convesion.TransactionAmount}";
                        processResult.AppendLine(logMessage);
                        _logger.LogInformation(logMessage, saleToDo);
                    }
                    await _cachingService.SetAsync(sale.Id.ToString(), JsonSerializer.Serialize(saleToDo), convesion.TimeLastUpdateUtc);
                    logMessage = $"Record {sale.AccountID} not fount in the database, so sent to the Redis cache";
                    processResult.AppendLine(logMessage);
                    _logger.LogInformation(logMessage, saleToDo);
                }

                return processResult.ToString();

            }
            catch (RedisException re)
            {
                _logger.LogError(re.Message, sale);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, sale);
                throw;
            }
        }

        /// <summary>
        /// To convert amount in US dollar if currency code already isn't is
        /// </summary>
        /// <param name="sale">The Sale entity</param>
        /// <returns>The amount value in Dollar</returns>
        static async Task<RateConvertionResult> ConvertToDollar(Sale sale)
        {
            try
            {
                var rateConvertionResult = new RateConvertionResult
                {
                    TimeLastUpdateUtc = DateTime.Now.AddHours(1),
                    TransactionAmount = sale.TransactionAmount
                };

                var URLString = new Uri($@"https://v6.exchangerate-api.com/v6/{Parameters.ExchangeRateApiKey}/latest/USD");
                using var webClient = new WebClient();
                var json = await webClient.DownloadStringTaskAsync(URLString);
                ApiConversionObject convesion = JsonSerializer.Deserialize<ApiConversionObject>(json);

                //Reflection to get related value from sale to apply conversion
                var rateValue = convesion.ConversionRates.GetType().GetProperty(sale.TransactionCurrencyCode).GetValue(convesion.ConversionRates, null);
                if (rateValue == null)
                    return rateConvertionResult;
                rateConvertionResult.TimeLastUpdateUtc = Convert.ToDateTime(convesion.TimeNextUpdateUtc);
                rateConvertionResult.TransactionAmount = Math.Round(sale.TransactionAmount / Convert.ToDouble(rateValue), 2);
                return rateConvertionResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
