using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;

namespace StoreApp.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly IProductRepository _orderLineRepo;

        public OrderController(IOrderRepository orderRepo, ILocationRepository locationRepo, IProductRepository orderLineRepo)
        {
            _orderRepo = orderRepo;
            _locationRepo = locationRepo;
            _orderLineRepo = orderLineRepo;
        }
        public IActionResult PlaceOrder()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(int id)
        {
            return View();
        }

    }
}
