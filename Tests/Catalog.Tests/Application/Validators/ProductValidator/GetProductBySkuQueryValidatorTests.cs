using Catalog.Application.Queries.ProductQuery;
using Catalog.Application.Validators.ProductValidator;
using FluentValidation.TestHelper;
using Xunit;

namespace Catalog.Tests.Application.Validators.ProductValidator
{
    public class GetProductBySkuQueryValidatorTests
    {
        private readonly GetProductBySkuQueryValidator _validator;

        public GetProductBySkuQueryValidatorTests()
        {
            _validator = new GetProductBySkuQueryValidator();
        }

        [Fact]
        public void Should_Pass_When_Sku_Is_Valid()
        {
            var query = new GetProductBySkuQuery("ABC-123_456");

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Should_Fail_When_Sku_Is_Empty(string invalidSku)
        {
            var query = new GetProductBySkuQuery(invalidSku);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.Sku);
        }

        [Fact]
        public void Should_Fail_When_Sku_Exceeds_Max_Length()
        {
            var longSku = new string('A', 21);
            var query = new GetProductBySkuQuery(longSku);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.Sku);
        }

        [Theory]
        [InlineData("abc@123#")]
        [InlineData("sku com espaço")]
        [InlineData("sku*invalid")]
        public void Should_Fail_When_Sku_Has_Invalid_Characters(string invalidSku)
        {
            var query = new GetProductBySkuQuery(invalidSku);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.Sku);
        }
    }
}
