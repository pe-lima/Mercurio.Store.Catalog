using Catalog.Application.DTOs.Product;
using Catalog.Application.Handlers.ProductHandler;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mercurio.Store.UnitTests.Application.Handlers.ProductHandler
{
    public class GetProductByIdQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IMapper<Product, ProductDto>> _mapperMock;

        private readonly GetProductByIdQueryHandler _handler;
        public GetProductByIdQueryHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper<Product, ProductDto>>();

            _handler = new GetProductByIdQueryHandler(_productRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductDto_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product("Test Product", new Description("Test Description"), new Price(100), new Sku("SKU123"), new Stock(10));
            var productDto = new ProductDto
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Sku = "SKU123",
                Price = 100,
                Stock = 10,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync(product);
            
            _mapperMock
                .Setup(m => m.ToTarget(product))
                .Returns(productDto);

            var query = new GetProductByIdQuery(productId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDto.Id, result.Id);
            Assert.Equal(productDto.Name, result.Name);
            Assert.Equal(productDto.Description, result.Description);
        }

        [Fact]
        public async Task Handle_ShouldThrowGlobalException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            
            _productRepositoryMock
                .Setup(repo => repo.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            var query = new GetProductByIdQuery(productId);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<GlobalException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal($"Product with id {productId} not found.", exception.Message);
            Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
        }
    }
}
