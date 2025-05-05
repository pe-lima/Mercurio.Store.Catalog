using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Commands.ProductCommand
{
    public record DeleteProductCommand(Guid Id) : IRequest<Unit>;
}
