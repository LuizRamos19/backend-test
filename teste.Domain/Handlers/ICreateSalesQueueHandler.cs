using System.Threading.Tasks;
using teste.Domain.Commands.Requests;
using teste.Domain.Commands.Response;

namespace teste.Domain.Handlers
{
    public interface ICreateSalesQueueHandler
    {
        Task<CreateSalesQueueResponse> Handle(CreateSalesQueueRequest request, string queueName);
    }
}
