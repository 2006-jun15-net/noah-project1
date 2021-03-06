﻿using System;
using System.Collections.Generic;

namespace StoreApp.Library.Model
{
    public class Store
    {
        private string _name;
        public int StoreId { get; set; }
        public string Name 
        {
            get => _name;
            set
            {
                if(value.Length == 0)
                {
                    throw new ArgumentException("Must have a name.", nameof(value));
                }
                _name = value;
            }
        }
        public Dictionary<Product, int> Inventory { get; set; } = new Dictionary<Product, int>();

        
    }
}
