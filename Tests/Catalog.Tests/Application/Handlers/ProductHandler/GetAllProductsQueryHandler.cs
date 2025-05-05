using Catalog.Application.DTOs.Product;
using Catalog.Application.Handlers.ProductHandler;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Tests.Application.Handlers.ProductHandler
{
    public class GetAllProductsQueryHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper<Product, ProductDto>> _mockMapper;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper<Product, ProductDto>>();
            _handler = new GetAllProductsQueryHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Return_Only_Active_Products_When_IncludeInactive_Is_False()
        {
            // Arrange
            var products = new List<Product>
            {
                new("Produto A", new("Descricao A"), new(100), new("SKU-A"), new(5)),
                new("Produto B", new("Descricao B"), new(150), new("SKU-B"), new(3))
            };
            products[1].SetDelete();

            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mockMapper
                .Setup(m => m.ToTarget(It.Is<Product>(p => p.IsActive)))
                .Returns<Product>(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Value,
                    Price = p.Price.Value,
                    Sku = p.Sku.Value,
                    Stock = p.Stock.Quantity,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });

            // Act
            var result = await _handler.Handle(new GetAllProductsQuery(false), CancellationToken.None);

            // Assert
            Assert.Single(result);
            Assert.All(result, p => Assert.True(p.IsActive));
            Assert.Equal("Produto A", result[0].Name);
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(It.IsAny<Product>()), Times.Exactly(1));
        }

        [Fact]
        public async Task Should_Return_All_Products_When_IncludeInactive_Is_True()
        {
            // Arrange
            var products = new List<Product>
            {
                new("Produto A", new("Descricao A"), new(100), new("SKU-A"), new(5)),
                new("Produto B", new("Descricao B"), new(150), new("SKU-B"), new(3))
            };
            products[1].SetDelete();

            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Product>()))
                .Returns<Product>(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Value,
                    Price = p.Price.Value,
                    Sku = p.Sku.Value,
                    Stock = p.Stock.Quantity,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });

            // Act
            var result = await _handler.Handle(new GetAllProductsQuery(true), CancellationToken.None);

            // Assert
            Assert.Equal(2, result.Count);
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(It.IsAny<Product>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Should_Return_Empty_List_When_No_Products_Exist()
        {
            // Arrange
            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Product>());

            // Act
            var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(It.IsAny<Product>()), Times.Never);
        }
    }
}
