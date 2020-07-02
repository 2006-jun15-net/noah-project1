using System;
using System.Collections.Generic;
using System.Text;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;

namespace StoreApp.DataAccess.Repos
{
    class LocationRepository : ILocationRepository, IProductRepository
    {
        public void Create(Store location)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Store> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            throw new NotImplementedException();
        }

        public void Update(Store location)
        {
            throw new NotImplementedException();
        }
    }
}
