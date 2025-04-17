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

namespace Catalog.Tests.Application.Handlers.ProductHandler
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IMapper<Product, ProductDto>> _mockMapper;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockMapper = new Mock<IMapper<Product, ProductDto>>();
            _handler = new UpdateProductCommandHandler(_mockRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Should_Update_Product_Successfully()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Camiseta",
                "Camiseta de algodao",
                "SKU-123",
                99.99m,
                10
            );

            var product = new Product(
                "Antigo",
                new Description("Desc antiga"),
                new Price(10),
                new Sku("OLD-1"),
                new Stock(2)
            );

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _mockMapper
                .Setup(m => m.ToTarget(product))
                .Returns(new ProductDto { Id = command.Id, Name = command.Name, Description = command.Description, Sku = command.Sku});

            var result = await _handler.Handle(command, CancellationToken.None);

            _mockRepository.Verify(r => r.Update(product), Times.Once);
            _mockMapper.Verify(m => m.ToTarget(product), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
        }

        [Fact]
        public async Task Should_Throw_When_Product_Not_Found()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Produto",
                "Descricao Base",
                "SKU-001",
                100,
                1
            );

            _mockRepository
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
        }
    }
}
