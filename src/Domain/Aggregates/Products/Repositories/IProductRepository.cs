namespace Domain.Aggregates.Products;

public interface IProductRepository
{
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
}