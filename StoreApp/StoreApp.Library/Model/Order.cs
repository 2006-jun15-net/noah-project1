using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoreApp.Library.Model
{
    public class Order
    {
        private Customer _customer;
        private Store _store;
        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; } = null;
        public Customer Customer 
        {
            get => _customer; 
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Orders must have a Customer.");
                }
                _customer = value;
            }
        }
        public Store Store 
        {
            get => _store;
            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Orders must have a Store.");
                }
                _store = value;
            }
        }
        public Dictionary<Product, int> OrderLine { get; set; } = new Dictionary<Product, int>();
        public decimal TotalCost { get; set; }
    
        public Order()
        {
        }
        public Order(int orderId, DateTime? orderDate, Store location, Customer customer, decimal totalCost)
        {
            OrderId = orderId;
            OrderDate = orderDate;
            Store = location;
            Customer = customer;
            TotalCost = totalCost;
        }
        public Order(Store location, DateTime orderDate, Dictionary<Product, int> orderLine, Customer customer, decimal totalCost)
        {
            Store = location;
            OrderDate = orderDate;
            OrderLine = orderLine;
            Customer = customer;
            TotalCost = totalCost;
        }
        public Order(ShoppingCart cart, Customer customer, decimal totalCost)
            : this(cart.Location, DateTime.Now, cart.Items, customer, totalCost)
        {
        }
    }
}
