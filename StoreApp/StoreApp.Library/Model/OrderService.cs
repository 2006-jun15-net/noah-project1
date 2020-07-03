﻿using StoreApp.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Library.Model
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;

        private readonly ILocationRepository _LocationRepo;

        public OrderService(IOrderRepository orderRepo, ILocationRepository locationRepo)
        {
            _orderRepo = orderRepo;
            _LocationRepo = locationRepo;
        }

        public Order PlaceOrder(ShoppingCart cart, Customer customer)
        {
            foreach(var product in cart.Items.Keys)
            {
                cart.Location.Inventory[product] -= cart.Items[product];
            }

            var order = new Order(cart, customer);

            _orderRepo.Create(order);
            _LocationRepo.Update(cart.Location);

            return order;
        }
    }
}
