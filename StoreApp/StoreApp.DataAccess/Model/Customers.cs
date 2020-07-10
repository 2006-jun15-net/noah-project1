﻿using System.Collections.Generic;

namespace StoreApp.DataAccess.Model
{
    public partial class Customers
    {
        public Customers()
        {
            Orders = new HashSet<Orders>();
        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
