using Confluent.Kafka;
using System.Threading.Tasks;
using teste.Models;

namespace teste.ApiCore31.Interfaces
{
    public interface IKafkaMessageRepository
    {
        Task<PersistenceStatus> SendMensagemAsync(Sale mensagem);
    }
}
