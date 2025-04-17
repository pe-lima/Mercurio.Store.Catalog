using Catalog.Application.DTOs.Product;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using MediatR;

namespace Catalog.Application.Handlers.ProductHandler
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper<Product, ProductDto> _mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper<Product, ProductDto> mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(cancellationToken);

            var filtered = request.IncludeInactive
                ? products
                : products.Where(p => p.IsActive);

            return filtered.Select(product => _mapper.ToTarget(product)).ToList();
        }
    }
}
