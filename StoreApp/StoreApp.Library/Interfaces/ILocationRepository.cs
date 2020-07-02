using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Library.Interfaces
{
    public interface ILocationRepository
    {
        IEnumerable<Store> GetAll();
        void Create(Store location);
        void Update(Store location);
    }
}
