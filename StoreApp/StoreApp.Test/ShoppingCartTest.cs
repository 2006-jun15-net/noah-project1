using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace StoreApp.Test
{
    public class ShoppingCartTest
    {

        [Fact]
        public void ShoppingGetsPassedNullLocationThrowsException()
        {
            Assert.ThrowsAny<ArgumentException>(() => new ShoppingCart(null));
        }

        [Fact]
        public void AddToCartThrowsExceptionWhenQtyIsGreaterThanStock()
        {
            //arrange
            
            Dictionary<Product, int> inv = new Dictionary<Product, int>();
            inv.Add(new Product { Name = "p1", Price = 100 }, 20);
            inv.Add(new Product { Name = "p2", Price = 200 }, 10);
            Store location = new Store
            {
                Name = "TestStore",
            };
            location.Inventory = inv;
            ShoppingCart cart = new ShoppingCart(location);

            //assert
            Assert.ThrowsAny<ArgumentException>(() => cart.AddToCart(location.Inventory.Keys.First(p => p.Name.Equals("p1")), 30)); 

        }

        [Fact]
        public void AddToCartFillsCartCorrectly()
        {
            //arrange
            
            var p1 = new Product { Name = "p1", Price = 100 };
            var p2 = new Product { Name = "p2", Price = 200 };
            Dictionary<Product, int> inv = new Dictionary<Product, int>();
            inv.Add(p1, 20);
            inv.Add(p2, 10);
            Store location = new Store
            {
                Name = "TestStore",
            };
            location.Inventory = inv;
            ShoppingCart cart = new ShoppingCart(location);
            int qty = 2;

            //act
            cart.AddToCart(location.Inventory.Keys.First(p => p.Name.Equals("p1")), qty);

            //assert
            Assert.NotNull(cart.Items.Keys.First(p => p.Name.Equals("p1")));
            Assert.Equal(qty, cart.Items[p1]);
        }

    }
}
