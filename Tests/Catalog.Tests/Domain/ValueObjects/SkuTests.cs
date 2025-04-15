using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using Xunit;

namespace Mercurio.Store.UnitTests.Domain.ValueObjects
{
    public class SkuTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Should_Throw_When_Sku_Is_Empty(string value)
        {
            Assert.Throws<DomainException>(() => new Sku(value));
        }

        [Fact]
        public void Should_Throw_When_Sku_Is_Too_Long()
        {
            var sku = new string('X', 21);
            Assert.Throws<DomainException>(() => new Sku(sku));
        }

        [Fact]
        public void Should_Create_Valid_Sku()
        {
            var sku = new Sku("ABC-123");
            Assert.Equal("ABC-123", sku.Value);
        }
    }
}
