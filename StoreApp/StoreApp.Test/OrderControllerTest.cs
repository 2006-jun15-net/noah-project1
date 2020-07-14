using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using StoreApp.WebApp.Controllers;
using StoreApp.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StoreApp.Test
{
    public class OrderControllerTest
    {
        private Mock<ICustomerRepository> customerRepo = new Mock<ICustomerRepository>();
        private Mock<IOrderRepository> orderRepo = new Mock<IOrderRepository>();
        private Mock<ILocationRepository> locationRepo = new Mock<ILocationRepository>();
        [Fact]
        public void OrderController_OrderHistory_ReturnsListOfOrders()
        {
            //arrange
            customerRepo.Setup(repo => repo.GetByUsername("TestTest"))
                .Returns(new Customer()
                {
                    CustomerId = 1,
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "TestTest"
                }) ;
            orderRepo.Setup(repo => repo.GetOrderHistory(It.IsAny<Customer>()))
                .Returns(new List<Order>
                {
                    new Order()
                    {
                        OrderId = 1,
                        OrderDate = DateTime.Now,
                        OrderLine = new Dictionary<Product, int>(),
                        Store = new Store{Name = "testStore"},
                        TotalCost = 1.00M
                    }

                });

            //act
            var controller = new OrderController(orderRepo.Object, locationRepo.Object, customerRepo.Object);
            var result = (ViewResult)controller.OrderHistory("TestTest");

            //assert
            Assert.IsType<List<OrderViewModel>>(result.Model);
        }

        [Fact]
        public void OrderController_Details_ReturnsOrderViewModel()
        {
            //arrange
            orderRepo.Setup(repo => repo.GetById(1))
                .Returns(new Order()
                {
                    OrderId = 1,
                    OrderDate = DateTime.Now,
                    OrderLine = new Dictionary<Product, int>(),
                    Store = new Store { Name = "testStore" },
                    Customer = new Customer() { UserName = "test"},
                    TotalCost = 1.00M
                });

            //act
            var controller = new OrderController(orderRepo.Object, locationRepo.Object, customerRepo.Object);
            var result = (ViewResult)controller.Details(1);

            //assert 
            Assert.IsType<OrderViewModel>(result.Model);
        }
    }
}
