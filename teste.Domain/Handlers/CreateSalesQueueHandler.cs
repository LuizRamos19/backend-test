using System.Text.Json;
using System.Threading.Tasks;
using teste.Domain.Commands.Requests;
using teste.Domain.Commands.Response;
using teste.Models.Anemic;
using teste.Repositories.Kafka;

namespace teste.Domain.Handlers
{
    public class CreateSalesQueueHandler : ICreateSalesQueueHandler
    {
        private readonly IKafkaRepository _kafkaRepository = new KafkaRepository();

        public async Task<CreateSalesQueueResponse> Handle(CreateSalesQueueRequest request, string queueName)
        {
            var processSteps = new ProcessSteps();
            processSteps.AddLogMessageContent($"Api received {(request.SalesList.Count == 1 ? "Sale record" : "Sales recors")} to queue up on Kafka");

            foreach (var sale in request.SalesList)
            {
                var status = await _kafkaRepository.SendMensagem(JsonSerializer.Serialize(sale), queueName);
                processSteps.AddLogMessageContent(
                    status == true
                        ? $"AccountID: {sale.AccountID} processed with status {status} in the queue."
                        : $"AccountID: {sale.AccountID} already in the queue."
                );
            }
            var result = new CreateSalesQueueResponse { ProcessSteps = processSteps.GetLogMessageContent() };
            return result;
        }
    }
}
