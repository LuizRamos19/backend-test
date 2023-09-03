using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using teste.ApiCore31.Constatns;
using teste.ApiCore31.Interfaces;
using teste.Models;

namespace teste.ApiCore31.Repositories
{
    public class KafkaMenssageRepository : IKafkaMessageRepository
    {
        public async Task<PersistenceStatus> SendMensagemAsync(Sale sale)
        {
            var config = new ProducerConfig { BootstrapServers = Parameters.KafkaHost };
            using var producer = new ProducerBuilder<string, string>(config).Build();
            var json = JsonSerializer.Serialize(sale);
            var status = await producer.ProduceAsync("queue_Kafka",
                new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = json
                }
            );
            return status.Status;
        }
    }
}
