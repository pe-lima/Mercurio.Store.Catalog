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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper<Product, ProductDto> _mapper;

        public UpdateProductCommandHandler(IProductRepository productRepository, IMapper<Product, ProductDto> mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken) 
                ?? throw new GlobalException($"Product with id '{request.Id}' not found.", HttpStatusCode.NotFound);
            product.Update(
                request.Name,
                new Description(request.Description),
                new Price(request.Price),
                new Sku(request.Sku),
                new Stock(request.Quantity)
            );

            _productRepository.Update(product);

            return _mapper.ToTarget(product);
        }
    }
}
