using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
