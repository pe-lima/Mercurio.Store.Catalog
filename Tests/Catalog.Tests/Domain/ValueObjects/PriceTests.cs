using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using Xunit;

namespace Mercurio.Store.UnitTests.Domain.ValueObjects
{
    public class PriceTests
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Throw_When_Price_Is_Negative(decimal value)
        {
            Assert.Throws<DomainException>(() => new Price(value));
        }

        [Fact]
        public void Should_Create_Valid_Price()
        {
            var price = new Price(99.90m);
            Assert.Equal(99.90m, price.Value);
        }
    }
}
