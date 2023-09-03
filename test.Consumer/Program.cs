using Confluent.Kafka;
using Snapper.Core.DataBase.Engines;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using teste.Models;

namespace test.Consumer
{
    internal class Program
    {
        static readonly CancellationTokenSource _cts = new CancellationTokenSource();

        static async Task Main(string[] args)
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
            Console.WriteLine("------------------------------------------------------");
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServer,
                GroupId = $"{kafkaQueueName}-group-0",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(kafkaQueueName);
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(_cts.Token);
                    if (cr != null)
                    {
                        var json = cr.Message.Value;
                        var sale = JsonSerializer.Deserialize<Sale>(json);
                        Console.WriteLine($"Sale Id: {sale.AccountID} processed");
                        Console.WriteLine("------------------------------------------------------");
                    }
                }
                catch (OperationCanceledException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    await KillConsoleAppAsync(ex.Message);
                    throw;
                }
            }
        }

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

        static async Task KillConsoleAppAsync(string message)
        {
            Console.WriteLine("Application generate exception an will close in 5 seconds");
            Console.WriteLine(message);
            for (int i = 5; i > 0; i--)
            {
                Console.WriteLine($"{i}");
                await Task.Delay(1000);
            }
            Environment.Exit(0);
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
                var URLString = new Uri($@"https://v6.exchangerate-api.com/v6/351b37bfc54dbe41826820d1/latest/USD");
                using var webClient = new System.Net.WebClient();
                var json = await webClient.DownloadStringTaskAsync(URLString);
                ApiConversionObject convesion = JsonSerializer.Deserialize<ApiConversionObject>(json);

                //Reflection to get related value from sale to apply conversion
                var rateValue = convesion.ConversionRates.GetType().GetProperty(sale.TransactionCurrencyCode).GetValue(convesion.ConversionRates, null);
                if (rateValue == null)
                    return sale;
                sale.TransactionAmount /= Convert.ToDouble(rateValue);
                return sale;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create native connection to Oracle data base.
        /// </summary>
        /// <returns>Snapper that contais connection ready for use</returns>
        static Snapper.Core.Snapper CreateOracleConnection() =>
            new Snapper.Core.Snapper("XE", "localhost", "system", "Teste123456", new OracleEngine());
    }
}
