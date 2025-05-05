using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using Xunit;

namespace Catalog.Tests.Domain.ValueObjects
{
    public class StockTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Throw_When_Stock_Is_Negative(int quantity)
        {
            Assert.Throws<DomainException>(() => new Stock(quantity));
        }

        [Fact]
        public void Should_Create_Valid_Stock()
        {
            var stock = new Stock(10);
            Assert.Equal(10, stock.Quantity);
        }

        [Fact]
        public void Should_Increase_Stock()
        {
            var stock = new Stock(5);
            var result = stock.Increase(3);
            Assert.Equal(8, result.Quantity);
        }

        [Fact]
        public void Should_Throw_When_Increasing_With_Invalid_Amount()
        {
            var stock = new Stock(5);
            Assert.Throws<DomainException>(() => stock.Increase(0));
            Assert.Throws<DomainException>(() => stock.Increase(-5));
        }

        [Fact]
        public void Should_Decrease_Stock()
        {
            var stock = new Stock(5);
            var result = stock.Decrease(3);
            Assert.Equal(2, result.Quantity);
        }

        [Fact]
        public void Should_Throw_When_Decreasing_With_Invalid_Amount()
        {
            var stock = new Stock(5);
            Assert.Throws<DomainException>(() => stock.Decrease(0));
            Assert.Throws<DomainException>(() => stock.Decrease(-1));
        }

        [Fact]
        public void Should_Throw_When_Decreasing_Below_Zero()
        {
            var stock = new Stock(2);
            Assert.Throws<DomainException>(() => stock.Decrease(3));
        }
    }
}
