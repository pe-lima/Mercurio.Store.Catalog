using Catalog.Application.DTOs.Product;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using MediatR;
using System.Net;

namespace Catalog.Application.Handlers.ProductHandler
{
    public class GetProductBySkuQueryHandler : IRequestHandler<GetProductBySkuQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper<Product, ProductDto> _mapper;

        public GetProductBySkuQueryHandler(IProductRepository productRepository, IMapper<Product, ProductDto> mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductBySkuQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetBySkuAsync(request.Sku) ??
                throw new GlobalException($"Product with sku {request.Sku} not found.", HttpStatusCode.NotFound);

            return _mapper.ToTarget(product);
        }
    }
}
