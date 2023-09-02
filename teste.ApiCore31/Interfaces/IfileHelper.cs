using System.Collections.Generic;
using teste.ApiCore31.Models;

namespace teste.ApiCore31.Interfaces
{
    public interface IFileHelper
    {
        public List<Sale> SalesListFromFile();
    }
}