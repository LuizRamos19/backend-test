using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using System.Text.Json;
using teste.Infrastructure.Sevices.ExchangeRate;
using teste.Models;
using teste.Models.Anemic;
using teste.Repositories.Redis;
using teste.Repositories.Sales;

namespace tes.Consumer2
{
    internal interface IApplication
    {
        Task RunAsync();
    }

    internal class Application : IApplication
    {
        private readonly ILogger<Application> _loggerApplication;
        private readonly ISaleRepository _saleRepository;
        private readonly IRedisRepository _redisRepository;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly CancellationTokenSource _cts = new();

        public Application(
            ILogger<Application> loggerApplication,
            ISaleRepository saleRepository, 
            IRedisRepository redisRepository, 
            IExchangeRateService exchangeRateService)
        {
            _loggerApplication = loggerApplication;
            _saleRepository = saleRepository;
            _redisRepository = redisRepository;
            _exchangeRateService = exchangeRateService;
        }

        public async Task RunAsync()
        {
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                _cts.Cancel();
            };

            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("-                   Process Kafka queue consuming jobs                  -");
            Console.WriteLine("-------------------------------------------------------------------------");
            Console.WriteLine("Fill Up bellow parameters:");

            #region-- Param configuration --
            var bootstrapServer = "";
            var kafkaQueueName = "";

            GetBootstrapServers(ref bootstrapServer);
            while (string.IsNullOrEmpty(bootstrapServer))
            {
                Console.WriteLine("Please, parameter can't be null!");
                GetBootstrapServers(ref bootstrapServer);
            }
            Console.WriteLine($"BootstrapServers defined as :{bootstrapServer}");
            Console.WriteLine("Next, please inform the kafka queue name.");
            GetKafkaQueueName(ref kafkaQueueName);
            while (string.IsNullOrEmpty(kafkaQueueName))
            {
                Console.WriteLine("Please, KafkaQueueName can't be null!");
                GetBootstrapServers(ref kafkaQueueName);
            }
            Console.WriteLine();
            Console.WriteLine("Listening will start...");
            Console.WriteLine($"Retrieving record from {kafkaQueueName}.");
            Console.WriteLine();
            #endregion


            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServer,
                GroupId = $"{kafkaQueueName}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(kafkaQueueName);

            try
            {
                var counter = 1;
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(_cts.Token);
                        if (consumeResult != null)
                        {
                            Console.WriteLine($"{counter:000000}------------------------------------------------");
                            Console.WriteLine("Retrieving record from Kafka.");
                            var json = consumeResult.Message.Value;
                            Sale? sale = JsonSerializer.Deserialize<Sale>(json);
                            var logContent = sale != null
                                ? $"Record {sale.AccountID} retrieved."
                                : "Nothing foud from deserealization, Weird :(";
                            _loggerApplication.LogInformation(logContent, sale);

                            var convesion = new RateConvertionResult();
                            if (sale != null)
                            {
                                if (sale.TransactionCurrencyCode != "USD")
                                {
                                    convesion = await _exchangeRateService.ProcessRateConvertion(sale);
                                    _loggerApplication.LogInformation($"Record {sale.AccountID} has Transaction Amount converted from {sale.TransactionCurrencyCode}: {sale.TransactionAmount} to USD: {convesion.TransactionAmount}");
                                    sale.TransactionAmount = convesion.TransactionAmount;
                                }
                                await _redisRepository.SetValue(consumeResult.Key, sale, Convert.ToString(convesion.TimeLastUpdateUtc));
                                _loggerApplication.LogInformation($"The record was persisted in the Redis Database with ID = {consumeResult.Key}");

                                sale = await _saleRepository.SaveSale(sale);
                                _loggerApplication.LogInformation($"The record was persisted in the Oracle Database with ID = {sale.Id}");
                                counter++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _loggerApplication.LogError(ex.Message, ex);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _loggerApplication.LogError(ex.Message, ex);
                throw;
            }
            finally
            {
                Console.WriteLine("Process finished");
            }
        }

        #region-- Promp filling up --
        static private void GetBootstrapServers(ref string param)
        {
            Console.Write("BootstrapServers?: ");
            param = Console.ReadLine();
        }

        static private void GetKafkaQueueName(ref string param)
        {
            Console.Write("KafkaQueueName?: ");
            param = Console.ReadLine();
        }
        #endregion

    }
}
