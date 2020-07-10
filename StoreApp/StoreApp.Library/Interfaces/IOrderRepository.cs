using StoreApp.Library.Model;
using System.Collections.Generic;

namespace StoreApp.Library.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order GetById(int id);
        Dictionary<Product, int> GetAllProducts(int id);
        void Create(Order order);
        IEnumerable<Order> GetOrderHistory(object model);
    }
}
