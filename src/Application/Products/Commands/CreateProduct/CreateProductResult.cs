namespace Application.Products.Commands.CreateProduct;

public class CreateProductResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}