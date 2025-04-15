using Catalog.Application.Commands.ProductCommand;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Mappers;
using Catalog.CrossCutting.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.ProductHandler
{
    public class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper<Product, ProductDto> _mapper;
        public RegisterProductCommandHandler(
            IProductRepository productRepository,
            IMapper<Product, ProductDto> mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ProductDto> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var skuExists = _productRepository.ExistsBySkuAsync(request.Sku);
            if (skuExists.Result)
                throw new GlobalException($"A product with SKU '{request.Sku}' already exists.", HttpStatusCode.Conflict);

            var product = new Product(
                request.Name,
                new Description(request.Description),
                new Price(request.Price),
                new Sku(request.Sku),
                new Stock(request.Quantity));

            await _productRepository.AddAsync(product);

            return _mapper.ToTarget(product);
        }
    }
}
