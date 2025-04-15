using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.ValueObjects;
using System;
using Xunit;

namespace Mercurio.Store.UnitTests.Domain.Entities
{
    public class ProductTests
    {
        [Fact]
        public void Should_Create_Product_Successfully()
        {
            var product = new Product(
                name: "Nike Sneakers",
                description: new Description("Comfortable running sneakers"),
                price: new Price(299.90m),
                sku: new Sku("TNK-001"),
                stock: new Stock(10)
            );

            Assert.Equal("Nike Sneakers", product.Name);
            Assert.Equal("Comfortable running sneakers", product.Description.Value);
            Assert.Equal(299.90m, product.Price.Value);
            Assert.Equal("TNK-001", product.Sku.Value);
            Assert.Equal(10, product.Stock.Quantity);
            Assert.True(product.IsActive);
            Assert.NotEqual(Guid.Empty, product.Id);
            Assert.NotEqual(default, product.CreatedAt);
            Assert.Null(product.UpdatedAt);
        }

        [Fact]
        public void Should_Update_Product_And_Set_UpdatedAt()
        {
            var product = new Product("Old Product", new Description("description Tests"), new Price(100), new Sku("ABC-001"), new Stock(5));

            var previousUpdate = product.UpdatedAt;

            product.Update(
                name: "Updated Product",
                description: new Description("New description"),
                price: new Price(200),
                sku: new Sku("ABC-002"),
                stock: new Stock(8)
            );

            Assert.Equal("Updated Product", product.Name);
            Assert.Equal("New description", product.Description.Value);
            Assert.Equal(200, product.Price.Value);
            Assert.Equal("ABC-002", product.Sku.Value);
            Assert.Equal(8, product.Stock.Quantity);
            Assert.True(product.UpdatedAt > product.CreatedAt);
        }

        [Fact]
        public void Should_Update_Price()
        {
            var product = new Product("Product", new Description("description Tests"), new Price(100), new Sku("ABC-001"), new Stock(5));

            product.UpdatePrice(new Price(150));

            Assert.Equal(150, product.Price.Value);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void Should_Increase_Stock()
        {
            var product = new Product("Product", new Description("description Tests"), new Price(100), new Sku("ABC-001"), new Stock(5));

            product.IncreaseStock(3);

            Assert.Equal(8, product.Stock.Quantity);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void Should_Decrease_Stock()
        {
            var product = new Product("Product", new Description("description Tests"), new Price(100), new Sku("ABC-001"), new Stock(10));

            product.DecreaseStock(4);

            Assert.Equal(6, product.Stock.Quantity);
            Assert.NotNull(product.UpdatedAt);
        }

        [Fact]
        public void Should_Throw_When_Decrease_Stock_Below_Zero()
        {
            var product = new Product("Product", new Description("description Tests"), new Price(100), new Sku("ABC-001"), new Stock(2));

            Assert.Throws<DomainException>(() => product.DecreaseStock(5));
        }

        [Fact]
        public void Should_Mark_Product_As_Deleted()
        {
            var product = new Product("Product", new Description("description Tests"), new Price(100), new Sku("ABC-001"), new Stock(1));

            product.SetDelete();

            Assert.False(product.IsActive);
            Assert.NotNull(product.UpdatedAt);
        }
    }
}
