using System;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.WebApp.ViewModels
{
    public class OrderLineViewModel
    {
        public int ProductId { get; set; }
        
        [Required]
        [Range(0,100)]
        public int qty { get; set; }
    }
}
