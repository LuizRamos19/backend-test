using System.Collections.Generic;
using teste.Models;

namespace teste.Domain.Commands.Requests
{
    public class CreateSalesQueueRequest
    {
        public List<Sale> SalesList { get; set; }

    }
}
