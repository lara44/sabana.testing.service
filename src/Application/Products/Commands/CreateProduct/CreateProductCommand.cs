using AtcMediator;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommand : IAtcRequest<CreateProductResult>
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
