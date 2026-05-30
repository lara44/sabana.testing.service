namespace sabana.testing.service.tests.unit.application.products.queries.getproducts;

using Application.Products.Queries.GetProducts;
using Domain.Aggregates.Products;

[TestClass]
public sealed class GetProductsQueryHandlerTests
{
    [TestMethod]
    public async Task Given_ProductsStoredInRepository_When_HandleAsync_Then_ReturnsMappedProducts()
    {
        // Arrange (Given)
        var cancellationTokenSource = new CancellationTokenSource();
        var products = new List<Product>
        {
            Product.Rehydrate(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Arroz", 1200m),
            Product.Rehydrate(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Frijol", 900m),
        };
        var repository = new FakeProductRepository(products);
        var handler = new GetProductsQueryHandler(repository);
        var query = new GetProductsQuery();

        // Act (When)
        var result = await handler.HandleAsync(query, cancellationTokenSource.Token);

        // Assert (Then)
        Assert.AreEqual(1, repository.GetAllAsyncCallCount);
        Assert.AreEqual(cancellationTokenSource.Token, repository.LastCancellationToken);
        Assert.HasCount(products.Count, result);

        var firstResult = result.ElementAt(0);
        var secondResult = result.ElementAt(1);

        Assert.AreEqual(products[0].Id, firstResult.Id);
        Assert.AreEqual(products[0].Name, firstResult.Name);
        Assert.AreEqual(products[0].Price, firstResult.Price);
        Assert.AreEqual(products[1].Id, secondResult.Id);
        Assert.AreEqual(products[1].Name, secondResult.Name);
        Assert.AreEqual(products[1].Price, secondResult.Price);
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        private readonly IReadOnlyCollection<Product> _products;

        public FakeProductRepository(IReadOnlyCollection<Product> products)
        {
            _products = products;
        }

        public int GetAllAsyncCallCount { get; private set; }

        public CancellationToken LastCancellationToken { get; private set; }

        public Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }

        public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            GetAllAsyncCallCount++;
            LastCancellationToken = cancellationToken;
            return Task.FromResult(_products);
        }
    }
}
