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
        private readonly OrderService _orderService;

        public OrderController(IOrderRepository orderRepo, ILocationRepository locationRepo, ICustomerRepository customerRepo)
        {
            _orderRepo = orderRepo;
            _locationRepo = locationRepo;
            _customerRepo = customerRepo;
            _orderService = new OrderService(_orderRepo, _locationRepo);
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
            else
            {
                ModelState.AddModelError("", "Invalid quantity.");
                return View(model);
            }
            

        }
        public IActionResult GetLocation()
        {
            var locations = _locationRepo.GetAll().ToList();

            ViewBag.locations = locations;

            return View();
        }

        [HttpPost]
        public IActionResult PlaceOrder(int StoreId)
        {
            var location = _locationRepo.GetById(StoreId);
            location.Inventory = _locationRepo.GetAllProducts(location.StoreId);
            ShoppingCart cart = new ShoppingCart(location);
            string username = (string)TempData["Customer"];
            Customer customer;

            //Check that user is signed in 
            try
            {
                customer = _customerRepo.GetByUsername(username);
            }
            catch (Exception)
            {
                TempData["errorMsg"] = "No customer for the order. Register or Sign in and try again.";
                return RedirectToAction(nameof(GetProducts), new { location.StoreId });
            }

            //Fill cart, if cart is empty return with error
            foreach (var product in location.Inventory.Keys)
            {
                if (TempData[product.Name] == null)
                {
                    TempData["errorMsg"] = "No products in order.";
                    return RedirectToAction(nameof(GetProducts), new { location.StoreId });
                }
                int qty = (int)TempData[product.Name];
                if (qty != 0)
                {
                    try
                    {
                        cart.AddToCart(product, qty);
                    }
                    catch (Exception ex)
                    {
                        TempData["errorMsg"] = ex.Message;
                        return RedirectToAction(nameof(GetProducts), new { location.StoreId });
                    }

                }
            }
            if (cart.Items.Count == 0)
            {
                TempData["errorMsg"] = "No products in order.";
                return RedirectToAction(nameof(GetProducts), new { location.StoreId });
            }

            //Create the order with order service then display customer order history
            try
            {
                
                _orderService.PlaceOrder(cart, customer);
                    
                return RedirectToAction(nameof(OrderHistory), new {username = customer.UserName });
            }
            catch (Exception)
            {
                TempData["errorMsg"] = "Error in placing an order.";
                return RedirectToAction(nameof(GetProducts), new { location.StoreId });
            }
               
        }

        public IActionResult OrderHistory(string username)
        {
            if(username == null)
            {
                ViewData["ErrorMsg"] = "No customer signed in to view history.";
                return View();
            }
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
        
        public IActionResult LocationOrderHistory()
        {
            ViewBag.locations = _locationRepo.GetAll();
            
            return View(new StoreViewModel());
        }

        [HttpPost]
        public IActionResult LocationOrderHistory(int StoreId)
        {
            ViewBag.locations = _locationRepo.GetAll();
            var location = _locationRepo.GetById(StoreId);
            StoreViewModel viewModel = new StoreViewModel
            {
                StoreId = location.StoreId,
                Name = location.Name,
            };
            var orderHistory = _orderRepo.GetOrderHistory(location);
            viewModel.OrderHistory = orderHistory.Select(o => new OrderViewModel
            { 
                OrderId = o.OrderId,
                OrderDate = (DateTime)o.OrderDate,
                Location = o.Store.Name,
                CustomerUsername = o.Customer.UserName,
                TotalCost = o.TotalCost
                
            }).ToList();
            return View(viewModel);

        }

        public IActionResult Details(int OrderId)
        {
            Order order = _orderRepo.GetById(OrderId);
            OrderViewModel orderDetails = new OrderViewModel
            {
                OrderId = order.OrderId,
                OrderDate = (DateTime)order.OrderDate,
                OrderLine = order.OrderLine,
                Location = order.Store.Name,
                CustomerUsername = order.Customer.UserName,
                TotalCost = order.TotalCost
            };
            return View(orderDetails);
            
        }

    }
}
