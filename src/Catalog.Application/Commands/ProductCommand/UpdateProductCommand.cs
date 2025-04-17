using Catalog.Application.DTOs.Product;
using MediatR;

namespace Catalog.Application.Commands.ProductCommand
{
    public record UpdateProductCommand(
        Guid Id,
        string Name,
        string Description,
        string Sku,
        decimal Price,
        int Quantity
    ) : IRequest<ProductDto>;
}
