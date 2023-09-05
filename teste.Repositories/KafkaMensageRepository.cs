using Confluent.Kafka;
using System;
using teste.Models;
using teste.Repositories.Interfaces;
using System.Text.Json;

namespace teste.Repositories
{
    public class KafkaMenssageRepository : IKafkaMessageRepository
    {
        public void SendMensagem(KafkaMessage mensagem)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                var json = JsonSerializer.Serialize(mensagem);
                producer.Produce("queue_Kafka", 
                    new Message<string, string> 
                    { 
                        Key = Guid.NewGuid().ToString(),
                        Value = json
                    }
                );
            }
        }
    }
}
