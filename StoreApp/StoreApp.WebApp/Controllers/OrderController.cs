﻿using System;
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
        public IActionResult PlaceOrder()
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