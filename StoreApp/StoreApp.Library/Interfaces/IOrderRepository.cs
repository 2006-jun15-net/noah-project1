using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Library.Interfaces
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Dictionary<Product, int> GetAllProducts(int id);
        void Create(Order order);
        IEnumerable<Order> GetOrderHistory(object model);
    }
}
