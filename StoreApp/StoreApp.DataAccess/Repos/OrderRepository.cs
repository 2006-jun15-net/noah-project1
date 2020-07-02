using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.DataAccess.Repos
{
    class OrderRepository : IOrderRepository, IProductRepository
    {
        public void Create(Order order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        
    }
}
