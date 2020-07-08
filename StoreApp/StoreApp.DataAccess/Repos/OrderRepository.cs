using Microsoft.EntityFrameworkCore;
using StoreApp.DataAccess.Model;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoreApp.DataAccess.Repos
{
    public class OrderRepository : IOrderRepository
    {
        private readonly _2006StoreApplicationContext _context;

        public OrderRepository(_2006StoreApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds an order to the database
        /// </summary>
        /// <param name="order">The order to be added</param>
        public void Create(Order order)
        {
            var entity = new Orders
            {
                OrderDate = order.OrderDate,
                CustomerId = order.Customer.CustomerId,
                StoreId = order.Store.StoreId,
                TotalCost = order.TotalCost
            };
            _context.Orders.Add(entity);
            _context.SaveChanges();

            foreach (var item in order.OrderLine.Keys)
            {
                entity.OrderLines.Add(new OrderLines
                {
                    ProductId = item.ProductId,
                    Amount = order.OrderLine[item]
                });
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// Gets all the orders without Order Lines
        /// </summary>
        /// <returns>The orders</returns>
        public IEnumerable<Order> GetAll()
        {
            var entities = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Store)
                .ToList();
            return entities.Select(e => new Order
                (
                    e.OrderId,
                    e.OrderDate,
                    new Store
                    {
                        StoreId = e.Store.StoreId,
                        Name = e.Store.StoreName,
                    },
                    new Customer
                    {
                        CustomerId = e.Customer.CustomerId,
                        FirstName = e.Customer.FirstName,
                        LastName = e.Customer.LastName,
                        UserName = e.Customer.UserName
                    },
                    e.TotalCost
                ));
        }
        /// <summary>
        /// Gets all products in the order
        /// </summary>
        /// <param name="id">The order id</param>
        /// <returns>A dictionary of products and their amounts</returns>
        public Dictionary<Product, int> GetAllProducts(int id)
        {
            var entity = _context.Orders
                    .Include(o => o.OrderLines)
                    .First(o => o.OrderId == id);
            Dictionary<Product, int> orderLines = new Dictionary<Product, int>();
            foreach (var item in entity.OrderLines)
            {
                var product = _context.Products.Find(item.ProductId);
                Product p = new Product
                {
                    ProductId = product.ProductId,
                    Name = product.ProductName,
                    Price = product.Price
                };
                orderLines.Add(p, item.Amount);
            }
            return orderLines;

        }

        /// <summary>
        /// Gets Order History
        /// </summary>
        /// <param name="id">the id</param>
        /// <returns>List of Orders</returns>
        public IEnumerable<Order> GetOrderHistory(object model)
        {
            if (model is Customer)
            {
                Customer customer = (Customer)model;
                var entities = _context.Orders
                    .Include(o => o.Store)
                    .Include(o => o.Customer)
                    .Where(o => o.CustomerId == customer.CustomerId);
                var orders = entities.Select(e => new Order
                (
                    e.OrderId,
                    e.OrderDate,
                    new Store
                    {
                        StoreId = e.Store.StoreId,
                        Name = e.Store.StoreName,
                    },
                    new Customer
                    {
                        CustomerId = e.Customer.CustomerId,
                        FirstName = e.Customer.FirstName,
                        LastName = e.Customer.LastName,
                        UserName = e.Customer.UserName
                    },
                    e.TotalCost
                ));
                foreach (var order in orders.ToList())
                {
                    order.OrderLine = GetAllProducts(order.OrderId);
                }
                return orders;
            }
            else if (model is Store)
            {
                Store store = (Store)model;
                var entities = _context.Orders
                    .Include(o => o.Store)
                    .Include(o => o.Customer)
                    .Where(o => o.CustomerId == store.StoreId);
                var orders = entities.Select(e => new Order
                (
                    e.OrderId,
                    e.OrderDate,
                    new Store
                    {
                        StoreId = e.Store.StoreId,
                        Name = e.Store.StoreName,
                    },
                    new Customer
                    {
                        CustomerId = e.Customer.CustomerId,
                        FirstName = e.Customer.FirstName,
                        LastName = e.Customer.LastName,
                        UserName = e.Customer.UserName
                    },
                    e.TotalCost
                ));
                foreach (var order in orders.ToList())
                {
                    order.OrderLine = GetAllProducts(order.OrderId);
                }
                return orders;
            }
            else
            {
                throw new ArgumentException("Can't get order history.");
            }


        }
    }
}
