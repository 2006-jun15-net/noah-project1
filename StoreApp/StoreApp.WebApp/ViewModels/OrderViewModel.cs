using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.WebApp.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string Location { get; set; }
        public DateTime OrderDate { get; set; }
        public Dictionary<Product, int> OrderLine { get; set; }
        public decimal TotalCost { get; set; }

    }
}
