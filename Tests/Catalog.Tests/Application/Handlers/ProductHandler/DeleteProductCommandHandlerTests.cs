using Catalog.Application.Commands.ProductCommand;
using Catalog.Application.Handlers.ProductHandler;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Exceptions;
using Catalog.Domain.Interfaces.Repositories;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.Tests.Application.Handlers.ProductHandler
{
    public class DeleteProductCommandHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _handler = new DeleteProductCommandHandler(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task Should_Delete_Product_When_Exists()
        {
            // Arrange
            var product = new Product("Produto X", new("Descrição X"), new(100), new("SKU-X"), new(5));
            var command = new DeleteProductCommand(product.Id);

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _productRepositoryMock
                .Setup(r => r.Update(It.IsAny<Product>()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(product.IsActive);
            _productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Product_Not_Found()
        {
            // Arrange
            var command = new DeleteProductCommand(Guid.NewGuid());

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<GlobalException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
