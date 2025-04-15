using Catalog.Application.Commands.ProductCommand;
using Catalog.Application.Validators.ProductValidator;
using FluentValidation.TestHelper;

namespace Mercurio.Store.UnitTests.Application.Validators.ProductValidator
{
    public class RegisterProductCommandValidatorTests
    {
        private readonly RegisterProductCommandValidator _validator;

        public RegisterProductCommandValidatorTests()
        {
            _validator = new RegisterProductCommandValidator();
        }

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            var command = new RegisterProductCommand("Produto A", "Descrição válida", "SKU-123", 99.99m, 10);

            var result = _validator.TestValidate(command);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Should_Fail_When_Price_Is_Invalid(decimal invalidPrice)
        {
            var command = new RegisterProductCommand("Produto", "Descrição", "SKU-001", invalidPrice, 5);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_When_Stock_Is_Negative(int invalidStock)
        {
            var command = new RegisterProductCommand("Produto", "Descrição", "SKU-001", 50m, invalidStock);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Quantity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_Name_Is_Invalid(string? invalidName)
        {
            var command = new RegisterProductCommand(invalidName, "Descrição", "SKU-001", 50m, 10);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Fail_When_Description_Is_Invalid(string? invalidDescription)
        {
            var command = new RegisterProductCommand("Produto", invalidDescription, "SKU-001", 50m, 10);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("SKU-EXCEEDING-MAX-LENGTH-1234567890")]
        public void Should_Fail_When_Sku_Is_Invalid(string? invalidSku)
        {
            var command = new RegisterProductCommand("Produto", "Descrição", invalidSku, 50m, 10);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.Sku);
        }
    }
}