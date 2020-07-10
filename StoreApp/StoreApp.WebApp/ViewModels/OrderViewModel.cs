using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp.ViewModels
{
    public class OrderViewModel
    {
        [Display(Name = "Order ID")]
        public int OrderId { get; set; }
        public string Location { get; set; }
        [Display(Name = "Username")]
        public string CustomerUsername { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        public Dictionary<Product, int> OrderLine { get; set; }
        [Display(Name = "Total Cost")]
        public decimal TotalCost { get; set; }

    }
}
