using StoreApp.Library.Model;
using System.Collections.Generic;

namespace StoreApp.Library.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Store> GetAll();
        Store GetById(int id);
        Dictionary<Product, int> GetAllProducts(int id);
        void Update(Store location);
    }
}
