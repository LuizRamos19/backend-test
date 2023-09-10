using System.Threading.Tasks;
using teste.Models;

namespace teste.Repositories.Sales
{
    public interface ISaleRepository
    {
        Task<Sale> GetSale(int id);

        Task<bool> SaleExists(int id);

        Task<Sale> SaveSale(Sale sale);
    }
}
