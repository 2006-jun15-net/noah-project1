﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using StoreApp.WebApp.ViewModels;

namespace StoreApp.WebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerRepository customerRepo, ILogger<CustomerController> logger)
        {
            _customerRepo = customerRepo;
            _logger = logger;
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
            Customer customer = _customerRepo.GetByUsername(username);
            var viewModel = new CustomerViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Username = customer.UserName
            };
            return View(viewModel);
        }

        public IActionResult Edit([FromRoute]string id)
        {
            Customer customer = _customerRepo.GetByUsername(id);
            var viewModel = new CustomerViewModel
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Username = customer.UserName
            };
            return View(viewModel);

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
