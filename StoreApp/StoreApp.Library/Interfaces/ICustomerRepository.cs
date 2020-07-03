﻿using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Library.Interfaces
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAll();
        void Register(Customer customer);
        void Remove(Customer customer);
        void Update(Customer customer);
    }
}