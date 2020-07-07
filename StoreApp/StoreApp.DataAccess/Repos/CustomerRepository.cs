using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoreApp.DataAccess.Model;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;

namespace StoreApp.DataAccess.Repos
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly _2006StoreApplicationContext _context;

        public CustomerRepository(_2006StoreApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all customers without order history
        /// </summary>
        /// <returns>The Customers</returns>
        public IEnumerable<Customer> GetAll()
        {
            var entities = _context.Customers.ToList();
            return entities.Select(e => new Customer
            {
                CustomerId = e.CustomerId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                UserName = e.UserName
            });
        }

        public Customer GetByUsername(string username)
        {
            Customers entity = _context.Customers.First(c => c.UserName.Equals(username));
            return new Customer
            {
                CustomerId = entity.CustomerId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName
            };
        }

        /// <summary>
        /// Adds a customer to the database
        /// </summary>
        /// <param name="customer">The customer to be added</param>
        public void Register(Customer customer)
        {
            var entity = new Customers
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                UserName = customer.UserName
            };

            _context.Customers.Add(entity);
            _context.SaveChanges();

        }

        /// <summary>
        /// Removes a customer from the database
        /// </summary>
        /// <param name="customer">The customer to be removed</param>
        public void Remove(Customer customer)
        {
            var entity = _context.Customers.First(c => c.UserName.Equals(customer.UserName));
            _context.Customers.Remove(entity);
            _context.SaveChanges();
        }

        /// <summary>
        /// Allows customer to change first name, last name, and username
        /// </summary>
        /// <param name="customer"></param>
        public void Update(Customer customer)
        {
            var entity = _context.Customers.First(c => c.CustomerId == customer.CustomerId);
            entity.FirstName = customer.FirstName;
            entity.LastName = customer.LastName;
            entity.UserName = customer.UserName;
            _context.Customers.Update(entity);
            _context.SaveChanges();
        }
    }
}
