using Catalog.Application.Commands.ProductCommand;
using Catalog.Application.DTOs.Product;
using Catalog.Application.Queries.ProductQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> RegisterProduct([FromBody] RegisterProductCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id), cancellationToken);
            return Ok(result);
        }

        [HttpGet("sku/{sku}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBySku(string sku, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetProductBySkuQuery(sku), cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken, [FromQuery] bool includeInactive = false)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(includeInactive), cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest("The id in the URL does not match the id in the body.");

            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
