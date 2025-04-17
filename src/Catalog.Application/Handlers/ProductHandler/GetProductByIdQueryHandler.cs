using Catalog.Application.DTOs.Product;
using Catalog.Application.Mappers;
using Catalog.Application.Queries.ProductQuery;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.ProductHandler
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper<Product, ProductDto> _mapper;

        public GetProductByIdQueryHandler(IProductRepository productRepository,IMapper<Product, ProductDto> mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken) ?? 
                throw new GlobalException($"Product with id {request.Id} not found.", HttpStatusCode.NotFound);

            return _mapper.ToTarget(product);
        }
    }
}
