using Confluent.Kafka;
using System;
using System.Threading.Tasks;

namespace teste.Repositories.Kafka
{
    public class KafkaRepository : IKafkaRepository
    {
        private static readonly string _bootstrapServers = "localhost:9092";

        public async Task<bool> SendMensagem(string mensagem, string queueName)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using var producer = new ProducerBuilder<string, string>(config).Build();
            await producer.ProduceAsync(
                queueName,
                new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = mensagem
                }
            );
            return true;
        }
    }
}
