using Catalog.Application.Commands.ProductCommand;
using FluentValidation;

namespace Catalog.Application.Validators.ProductValidator
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product ID is required.")
                .Must(id => id != Guid.Empty).WithMessage("Product ID cannot be empty.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MinimumLength(3).WithMessage("Product name must have at least 3 characters.")
                .MaximumLength(100).WithMessage("Product name must be up to 100 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MinimumLength(10).WithMessage("Description must have at least 10 characters.")
                .MaximumLength(500).WithMessage("Description must be up to 500 characters.");

            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(20).WithMessage("SKU must be up to 20 characters.")
                .Matches("^[A-Z0-9\\-_]+$").WithMessage("SKU must contain only uppercase letters, numbers, dashes and underlines.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
        }
    }
}
