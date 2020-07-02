using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Library.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();

    }
}
