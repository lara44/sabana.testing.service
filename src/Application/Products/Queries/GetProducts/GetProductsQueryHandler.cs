using AtcMediator;
using Domain.Aggregates.Products;

namespace Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IAtcRequestHandler<GetProductsQuery, IReadOnlyCollection<GetProductsResult>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyCollection<GetProductsResult>> HandleAsync(GetProductsQuery request, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetAllAsync(cancellationToken);

        return products
            .Select(product => new GetProductsResult
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            })
            .ToList();
    }
}