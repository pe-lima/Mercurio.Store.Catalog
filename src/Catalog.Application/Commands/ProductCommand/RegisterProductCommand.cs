using Catalog.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Commands.ProductCommand
{
    public record RegisterProductCommand
    (
        string Name,
        string Description,
        string Sku,
        decimal Price,
        int Quantity
    ) : IRequest<ProductDto>;
}
