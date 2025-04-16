using Catalog.Application.Validators.ProductValidator;
using Catalog.Application.Queries.ProductQuery;
using FluentValidation.TestHelper;

namespace Mercurio.Store.UnitTests.Application.Validators.ProductValidator
{
    public class GetProductByIdQueryValidatorTests
    {
        private readonly GetProductByIdQueryValidator _validator;
        public GetProductByIdQueryValidatorTests()
        {
            _validator = new GetProductByIdQueryValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            // Arrange
            var query = new GetProductByIdQuery(Guid.Empty);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                .WithErrorMessage("Product ID is required.");
        }

        [Fact]
        public void Should_Not_Have_Error_When_Id_Is_Valid_Guid()
        {
            // Arrange
            var query = new GetProductByIdQuery(Guid.NewGuid());

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Id);
        }
    }
}
