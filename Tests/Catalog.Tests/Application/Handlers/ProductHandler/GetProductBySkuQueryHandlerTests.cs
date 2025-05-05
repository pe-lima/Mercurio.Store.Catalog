using Catalog.Application.DTOs.Product;
using Catalog.Application.Handlers.ProductHandler;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.ValueObjects;
using Moq;
using System.Net;

namespace Catalog.Tests.Application.Handlers.ProductHandler
{
    public class GetProductBySkuQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper<Product, ProductDto>> _mapperMock;
        
        private readonly GetProductBySkuQueryHandler _handler;

        public GetProductBySkuQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper<Product, ProductDto>>();

            _handler = new GetProductBySkuQueryHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto_WhenProductExists()
        {
            // Arrange
            var sku = "SKU_123";
            var product = new Product("Test Product", new Description("Test Description"), new Price(100), new Sku("SKU_123"), new Stock(10));
            var productDto = new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Sku = sku,
                Price = 100,
                Stock = 10,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _productRepositoryMock
                .Setup(repo => repo.GetBySkuAsync(sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(m => m.ToTarget(product))
                .Returns(productDto);

            var query = new GetProductBySkuQuery(sku);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Description, result.Description);
            Assert.Equal(productDto.Sku, result.Sku);
        }

        [Fact]
        public async Task Handle_ShouldThrowGlobalException_WhenProductDoesNotExist()
        {
            // Arrange
            var sku = "SKU_NOT_FOUND";

            _productRepositoryMock
                .Setup(repo => repo.GetBySkuAsync(sku, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var query = new GetProductBySkuQuery(sku);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<GlobalException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal($"Product with sku {sku} not found.", exception.Message);
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }

    }
}
