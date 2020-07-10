using StoreApp.Library.Model;
using System;
using Xunit;

namespace StoreApp.Test
{

    public class OrderTest
    {
        Order order = new Order();
        [Fact]
        public void OrderIdShouldDefaultToZero()
        {
            Assert.Equal(0, order.OrderId);
        }
        [Fact]
        public void OrderDateShouldDefaultToNull()
        {
            Assert.Null(order.OrderDate);
        }
        [Fact]
        public void OrderCustomerIsNullShouldThrowException()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => order.Customer = null);
        }
        [Fact]
        public void StoreIsNullShouldThrowException()
        {
            Assert.ThrowsAny<ArgumentNullException>(() => order.Store = null);
        }
        [Fact]
        public void OrderLineIsEmptyInitially()
        {
            Assert.Empty(order.OrderLine);
        }
        
    }

}
