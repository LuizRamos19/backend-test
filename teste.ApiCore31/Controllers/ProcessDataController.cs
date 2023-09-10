using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using teste.Domain.Commands.Requests;
using teste.Domain.Commands.Response;
using teste.Domain.Handlers;
using Swashbuckle.AspNetCore.Annotations;

namespace teste.ApiCore31.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcessDataController : ControllerBase
    {
        private readonly ILogger<ProcessDataController> _logger;

        public ProcessDataController(ILogger<ProcessDataController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Receive some data to process in queue
        /// </summary>
        /// <param name="handler">The handler that will deal with the received data</param>
        /// <param name="command">The received data</param>
        /// <param name="queueName">The name that the queue should have</param>
        /// <returns>Log processing</returns>
        /// <remarks>
        /// Nothing herere
        /// </remarks>
        [HttpPost("createqueue/{queueName}")]
        [SwaggerOperation(Summary = "Process sale data", Description = "Receive some data to process in queue")]
        [SwaggerResponse(200, "Everything works well")]
        [SwaggerResponse(400, "BAD REQUEST")]
        [SwaggerResponse(500, "SERVER ERROR")]
        public async Task<CreateSalesQueueResponse> CreateQueue(
            [FromServices]ICreateSalesQueueHandler handler,
            [FromBody]CreateSalesQueueRequest command,
            [FromRoute(Name = "queueName")] string queueName)
        {
            var processStep = await handler.Handle(command, queueName);
            _logger.LogInformation($"Api received {(command.SalesList.Count == 1 ? "record" : "recors")} to process", command.SalesList);
            _logger.LogInformation(processStep.ProcessSteps);
            return processStep;
        }
    }
}