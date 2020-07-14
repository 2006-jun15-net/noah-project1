using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using StoreApp.DataAccess.Model;
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
    public class CustomerControllerTest
    {
        [Fact]
        public void CustomerController_Details_ReturnsACustomerViewModel()
        {
            var httpContext = new DefaultHttpContext();
            //arrange
            var customerRepo = new Mock<ICustomerRepository>();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            customerRepo.Setup(repo => repo.GetByUsername("TestTest"))
                .Returns(new Customer()
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "TestTest"
                });
            //act
            var controller = new CustomerController(customerRepo.Object)
            {
                TempData = tempData
            };

            var result = (ViewResult)controller.Details("TestTest");

            //assert
            Assert.IsType<CustomerViewModel>(result.Model);
        }
        [Fact]
        public void CustomerController_Edit_ReturnsACustomerViewModel()
        {
            var httpContext = new DefaultHttpContext();
            //arrange
            var customerRepo = new Mock<ICustomerRepository>();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            customerRepo.Setup(repo => repo.GetByUsername("TestTest"))
                .Returns(new Customer()
                {
                    FirstName = "Test",
                    LastName = "Test",
                    UserName = "TestTest"
                });

            //act 
            var controller = new CustomerController(customerRepo.Object)
            {
                TempData = tempData
            };
            var result = (ViewResult)controller.Edit("TestTest");

            //assert
            Assert.IsType<CustomerViewModel>(result.Model);
                
        }
        
    }
}
