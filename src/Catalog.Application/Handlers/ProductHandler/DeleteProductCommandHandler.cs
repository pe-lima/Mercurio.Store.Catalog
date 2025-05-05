using Catalog.Application.Commands.ProductCommand;
using Catalog.CrossCutting.Exceptions;
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
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
    {
        private readonly IProductRepository _productRepository;
        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id) ??
                throw new GlobalException($"Product with id {request.Id} not found.", HttpStatusCode.NotFound);

            product.SetDelete();

            _productRepository.Update(product);
            
            return Unit.Value;
        }
    }
}
