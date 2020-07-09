using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using StoreApp.WebApp.ViewModels;

namespace StoreApp.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ILocationRepository _locationRepo;

        public OrderController(IOrderRepository orderRepo, ILocationRepository locationRepo)
        {
            _orderRepo = orderRepo;
            _locationRepo = locationRepo;
        }
        
        public IActionResult GetProducts(int StoreId)
        {
            var model = _locationRepo.GetById(StoreId);
            model.Inventory = _locationRepo.GetAllProducts(model.StoreId);
            return View(model);
        }
        [HttpPost]
        public IActionResult GetProducts(int StoreId, [Bind("ProductId","qty")] OrderLineViewModel viewModel)
        {
            var model = _locationRepo.GetById(StoreId);
            model.Inventory = _locationRepo.GetAllProducts(model.StoreId);
            if (ModelState.IsValid)
            {
                decimal Total = 0;
                foreach(var product in model.Inventory.Keys)
                {
                    if(TempData[product.Name] == null)
                    {
                        TempData[product.Name] = 0;
                    }
                    
                    int count = (int)TempData[product.Name];
                    if (product.ProductId == viewModel.ProductId)
                    {
                        count += viewModel.qty;
                        TempData[product.Name] = count; 
                    }
                    TempData.Keep(product.Name);
                   
                    Total += product.Price*(int)TempData[product.Name];
                }
                ViewData["Total"] = Total;

                return View(model);
            }
            return View(model);

        }
        public IActionResult GetLocation()
        {
            var locations = _locationRepo.GetAll().ToList();

            ViewBag.locations = locations;

            return View();
        }

        

        //[HttpPost]
        //public IActionResult PlaceOrder(int id)
        //{
        //    return View();
        //}

    }
}
