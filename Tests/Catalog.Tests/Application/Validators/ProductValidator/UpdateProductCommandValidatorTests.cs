using Catalog.Application.Commands.ProductCommand;
using Catalog.Application.Validators.ProductValidator;
using FluentValidation.TestHelper;
using System;
using Xunit;

namespace Catalog.Tests.Application.Validators.ProductValidator
{
    public class UpdateProductCommandValidatorTests
    {
        private readonly UpdateProductCommandValidator _validator;

        public UpdateProductCommandValidatorTests()
        {
            _validator = new UpdateProductCommandValidator();
        }

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var command = new UpdateProductCommand(
                Guid.NewGuid(),
                "Produto Teste",
                "Descricao completa do produto",
                "SKU-001",
                99.90m,
                10
            );

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_Name_Is_Invalid(string name)
        {
            var command = new UpdateProductCommand(Guid.NewGuid(), name, "Descricao", "SKU-001", 10, 5);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Fail_When_Description_Is_Too_Short()
        {
            var command = new UpdateProductCommand(Guid.NewGuid(), "Produto", "Curta", "SKU-001", 10, 5);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Fail_When_Sku_Has_Invalid_Format()
        {
            var command = new UpdateProductCommand(Guid.NewGuid(), "Produto", "Descricao ok", "sku com espaço", 10, 5);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Sku);
        }

        [Fact]
        public void Should_Fail_When_Price_Is_Zero_Or_Negative()
        {
            var command = new UpdateProductCommand(Guid.NewGuid(), "Produto", "Descricao ok", "SKU-001", 0, 5);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Fail_When_Stock_Is_Negative()
        {
            var command = new UpdateProductCommand(Guid.NewGuid(), "Produto", "Descricao ok", "SKU-001", 10, -1);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }
    }
}
