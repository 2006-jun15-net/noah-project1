using StoreApp.Library.Model;
using System.Collections.Generic;

namespace StoreApp.Library.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        Customer GetByUsername(string username);
        void Register(Customer customer);
        void Remove(Customer customer);
        void Update(Customer customer);
    }
}
