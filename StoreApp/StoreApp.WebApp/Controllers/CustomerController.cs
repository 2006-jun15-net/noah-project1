using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using StoreApp.WebApp.ViewModels;

namespace StoreApp.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
      
        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;

        }

       
        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterCustomer([Bind("FirstName, LastName, Username")]CustomerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            try
            {
                Customer customer = new Customer 
                { 
                    FirstName = viewModel.FirstName, 
                    LastName = viewModel.LastName,
                    UserName = viewModel.Username
                };

                _customerRepo.Register(customer);
                TempData["Customer"] = customer.UserName;
                return RedirectToAction(nameof(Details), new { username = customer.UserName });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "there was some error, try again.");
                return View();
            }
        }

        public IActionResult Details(string username)
        {
            try
            {
                Customer customer = _customerRepo.GetByUsername(username);
                var viewModel = new CustomerViewModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Username = customer.UserName
                };
                TempData["Customer"] = customer.UserName;
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMsg"] = "User not found.";
                return RedirectToAction(nameof(Search));
            }
            
        }

        public IActionResult Edit(string username)
        {
            try
            {
                Customer customer = _customerRepo.GetByUsername(username);
                var viewModel = new CustomerViewModel
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Username = customer.UserName
                };
                return View(viewModel);
            }
            catch (Exception)
            {
                TempData["ErrorMsg"] = "User not found.";
                return RedirectToAction(nameof(Search));
            }
            

        }

        [HttpPost] 
        public IActionResult Edit([Bind("FirstName, LastName, Username")] CustomerViewModel viewModel, [FromRoute] string id )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Customer customer = _customerRepo.GetByUsername(id);
                    customer.FirstName = viewModel.FirstName;
                    customer.LastName = viewModel.LastName;
                    customer.UserName = viewModel.Username;
                    _customerRepo.Update(customer);

                    return RedirectToAction(nameof(Details), new { username = customer.UserName });
                }
                return View(viewModel);
            }
            catch (Exception)
            {
                return View(viewModel);
            }

        }

        public IActionResult Search(string search = null)
        {
            if(search != null)
            {
                if(_customerRepo.GetAll().Any(c => c.UserName.Equals(search)))
                {
                    return RedirectToAction(nameof(Details), new {username = search });
                }
                return View();            
            }
            return View();
        }
    }
}
