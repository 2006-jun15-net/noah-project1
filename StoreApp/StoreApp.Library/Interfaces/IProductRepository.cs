using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Library.Interfaces
{
    public interface IProductRepository
    {
        Dictionary<Product, int> GetAllProducts(int id);

    }
}
