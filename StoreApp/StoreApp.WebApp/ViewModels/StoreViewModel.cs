using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.WebApp.ViewModels
{
    public class StoreViewModel
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public List<OrderViewModel> OrderHistory { get; set; }
    }
}
