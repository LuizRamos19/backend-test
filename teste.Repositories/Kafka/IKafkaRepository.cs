using System.Collections.Generic;
using System.Threading.Tasks;

namespace teste.Repositories.Kafka
{
    public interface IKafkaRepository
    {
        Task<bool> SendMensagem(string mensagem, string queueName);
    }
}
