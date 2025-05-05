using Catalog.Application.DTOs.Product;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries.ProductQuery
{
    public record GetProductBySkuQuery(string Sku) : IRequest<ProductDto>;
}
