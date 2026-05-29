using Application.Products.Commands.CreateProduct;
using Application.Products.Queries.GetProducts;
using AtcMediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

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
    public async Task<ActionResult<ResponseDto<CreateProductResult>>> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.ExecuteAsync(command);
        var response = ResponseDto<CreateProductResult>.Success("Producto creado correctamente.", result);
        response.Code = StatusCodes.Status201Created;
        return StatusCode(StatusCodes.Status201Created, response);
    }

    [HttpGet]
    public async Task<ActionResult<ResponseDto<IReadOnlyCollection<GetProductsResult>>>> GetAll()
    {
        var result = await _mediator.ExecuteAsync(new GetProductsQuery());
        return Ok(ResponseDto<IReadOnlyCollection<GetProductsResult>>.Success(data: result));
    }
}
