namespace Application.Products.Queries.GetProducts;

public class GetProductsResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}