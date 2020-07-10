using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.WebApp.ViewModels
{
    public class ShoppingCartViewModel
    {
        public Dictionary<Product, int> Items { get; set; }
        public int StoreId{ get; set; }
        public decimal _cost;
        public decimal Cost
        {
            get
            {
                foreach(var item in Items.Keys)
                {
                    _cost += item.Price * Items[item];
                }
                return _cost;
            }
            set => _cost = value;
        }
    }
}
