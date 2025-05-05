using Catalog.Application.Queries.ProductQuery;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Validators.ProductValidator
{
    public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
    {
        public GetProductByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Product ID is required.")
                .Must(x => Guid.TryParse(x.ToString(), out _)).WithMessage("Invalid Product ID format.");
        }
    }
}
