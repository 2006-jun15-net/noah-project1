using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using StoreApp.WebApp.ViewModels;

namespace StoreApp.WebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ILocationRepository _locationRepo;
        private readonly ICustomerRepository _customerRepo;

        public OrderController(IOrderRepository orderRepo, ILocationRepository locationRepo, ICustomerRepository customerRepo)
        {
            _orderRepo = orderRepo;
            _locationRepo = locationRepo;
            _customerRepo = customerRepo;
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
                        if(count > model.Inventory[product])
                        {
                            ModelState.AddModelError("", "Not enough stock available for request");
                        }
                        else
                        {
                            TempData[product.Name] = count;
                        }
                         
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

        //public IActionResult PlaceOrder(int StoreId)
        //{
        //    var model = _locationRepo.GetById(StoreId);
        //    model.Inventory = _locationRepo.GetAllProducts(model.StoreId);
        //    ShoppingCart cart = new ShoppingCart(model);
        //    foreach(var product in model.Inventory.Keys)
        //    {
        //        int qty = (int)TempData[product.Name];
        //        if(qty != 0)
        //        {
        //            try
        //            {
        //                cart.AddToCart(product, qty);
        //            }
        //            catch (Exception ex)
        //            {
        //                ModelState.AddModelError("", ex.Message);
        //                break;
        //            }
                    
        //        }
        //    }
        //    return View(new ShoppingCartViewModel { Items = cart.Items, StoreId = model.StoreId});

        //}

        [HttpPost]
        public IActionResult PlaceOrder(int StoreId)
        {
            var location = _locationRepo.GetById(StoreId);
            location.Inventory = _locationRepo.GetAllProducts(location.StoreId);
            ShoppingCart cart = new ShoppingCart(location);
            foreach (var product in location.Inventory.Keys)
            {
                int qty = (int)TempData[product.Name];
                if (qty != 0)
                {
                    try
                    {
                        cart.AddToCart(product, qty);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                        return RedirectToAction(nameof(GetProducts), new { location.StoreId });
                    }

                }
            }
            
            string username = (string)TempData["Customer"];
            Customer customer;
            try
            {
                customer = _customerRepo.GetByUsername(username);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No customer for the order. Register or Sign in and try again.");
                return RedirectToAction(nameof(Index));
            }

            try
            {
                OrderService os = new OrderService(_orderRepo, _locationRepo);
                Order o = os.PlaceOrder(cart, customer);
                    
                return RedirectToAction(nameof(Details), new {username = customer.UserName });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error in placing an order.");
                return View();
            }
               
        }

        public IActionResult Details(string username)
        {
            var orderHistory = _orderRepo.GetOrderHistory(_customerRepo.GetByUsername(username));
            List<OrderViewModel> viewModels = new List<OrderViewModel>();
            foreach(var order in orderHistory)
            {
                viewModels.Add(new OrderViewModel
                {
                    OrderDate = (DateTime)order.OrderDate,
                    OrderId = order.OrderId,
                    OrderLine = order.OrderLine,
                    Location = order.Store.Name,
                    TotalCost = order.TotalCost
                });
            }
            return View(viewModels);
        }

    }
}
