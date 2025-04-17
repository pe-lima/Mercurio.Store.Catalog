using Catalog.Application.DTOs.Product;
using Catalog.Application.Handlers.ProductHandler;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Moq;

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
        public async Task Should_Return_All_Products()
        {
            // Arrange
            var products = new List<Product>
            {
                new("Produto A", new("Descricao A"), new(100), new("SKU-A"), new(5)),
                new("Produto B", new("Descricao B"), new(150), new("SKU-B"), new(3))
            };

            var productDtos = products.Select(p => new ProductDto
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
            }).ToList();

            _mockRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Product>()))
                .Returns<Product>(p => productDtos.First(d => d.Name == p.Name));

            // Act
            var result = await _handler.Handle(new GetAllProductsQuery(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Produto A", result[0].Name);
            Assert.Equal("Produto B", result[1].Name);

            _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(It.IsAny<Product>()), Times.Exactly(products.Count));
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
