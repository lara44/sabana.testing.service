using Application.Products.Commands.CreateProduct;
using AtcMediator;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Products;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IAtcMediator _mediator;

    public ProductController(IAtcMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.ExecuteAsync(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}
