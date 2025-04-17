using Catalog.Application.Queries.ProductQuery;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Validators.ProductValidator
{
    public class GetProductBySkuQueryValidator : AbstractValidator<GetProductBySkuQuery>
    {
        public GetProductBySkuQueryValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty().WithMessage("SKU is required.")
                .MaximumLength(20).WithMessage("SKU must be up to 20 characters.")
                .Matches("^[A-Z0-9\\-_]+$").WithMessage("SKU must contain only uppercase letters, numbers, dashes and underlines.");
        }
    }
}
