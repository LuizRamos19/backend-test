using teste.Models;

namespace teste.Repositories.Interfaces
{
    internal interface IKafkaMessageRepository
    {
        void SendMensagem(KafkaMessage mensagem);
    }
}
