using System;
using System.Linq;
using Xunit;

namespace Store.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Order_WithNullItems_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Order(1, null));
        }

        [Fact]
        public void TotalCount_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new OrderItem[0]);

            Assert.Equal(0, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithEmptyItems_ReturnsZero()
        {
            var order = new Order(1, new OrderItem[0]);

            Assert.Equal(0m, order.TotalPrice);
        }

        [Fact]
        public void TotalCount_WithNonEmptyItems_CalculatesTotalCount()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());
            order.Items.Add(bookId: 1, 10m, 3);
            order.Items.Add(bookId: 2, 100m, 5);

            Assert.Equal(3 + 5, order.TotalCount);
        }

        [Fact]
        public void TotalPrice_WithNonEmptyItems_CalculatesTotalPrice()
        {
            var order = new Order(1, Enumerable.Empty<OrderItem>());
            order.Items.Add(1, 10m, 3);
            order.Items.Add(2, 100m, 5);

            Assert.Equal(3 * 10m + 5 * 100m, order.TotalPrice);
        }
    }
}
