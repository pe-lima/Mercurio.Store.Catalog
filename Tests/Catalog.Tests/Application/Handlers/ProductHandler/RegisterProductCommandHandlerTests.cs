using Catalog.Application.Commands.ProductCommand;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Handlers.ProductHandler;
using Catalog.Application.Mappers;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.ValueObjects;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Mercurio.Store.UnitTests.Application.Handlers.ProductHandler
{
    public class RegisterProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockProductRepo;
        private readonly Mock<IMapper<Product, ProductDto>> _mockMapper;
        private readonly RegisterProductCommandHandler _handler;

        public RegisterProductCommandHandlerTests()
        {
            _mockProductRepo = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper<Product, ProductDto>>();
            _handler = new RegisterProductCommandHandler(_mockProductRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Register_Product_Successfully()
        {
            var command = new RegisterProductCommand("Tênis Nike", "Tênis para corrida", "TNK-001", 299.99m, 10);

            var product = new Product(
                command.Name,
                new Description(command.Description),
                new Price(command.Price),
                new Sku(command.Sku),
                new Stock(command.Quantity)
            );

            var expectedDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description.Value,
                Price = product.Price.Value,
                Sku = product.Sku.Value,
                Stock = product.Stock.Quantity,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };

            _mockProductRepo
                .Setup(r => r.ExistsBySkuAsync(command.Sku))
                .ReturnsAsync(false);

            _mockProductRepo
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Product>()))
                .Returns(expectedDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Sku, result.Sku);
            Assert.Equal(command.Price, result.Price);
            Assert.Equal(command.Quantity, result.Stock);

            _mockProductRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Sku_Already_Exists()
        {
            var command = new RegisterProductCommand("Produto Duplicado", "Descricao", "SKU-001", 100.0m, 5);

            _mockProductRepo
                .Setup(r => r.ExistsBySkuAsync(command.Sku))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<GlobalException>(() => _handler.Handle(command, CancellationToken.None));

            _mockProductRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_When_Invalid_VO_Is_Created()
        {
            var command = new RegisterProductCommand("Produto", "Descricao", "SKU-001", -50m, 5);

            _mockProductRepo
                .Setup(r => r.ExistsBySkuAsync(command.Sku))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Throw_When_Mapper_Fails()
        {
            var command = new RegisterProductCommand("Produto", "Descrição válida", "SKU-999", 150.0m, 5);

            _mockProductRepo
                .Setup(r => r.ExistsBySkuAsync(command.Sku))
                .ReturnsAsync(false);

            _mockProductRepo
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            _mockMapper
                .Setup(m => m.ToTarget(It.IsAny<Product>()))
                .Throws(new Exception("Mapping failed"));

            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));

            _mockProductRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(It.IsAny<Product>()), Times.Once);
        }
    }
}
