namespace sabana.testing.service.tests.unit.application.products.commands.createproduct;

using Application.Products.Commands.CreateProduct;
using Domain.Aggregates.Products;

[TestClass]
public sealed class CreateProductCommandHandlerTests
{
    [TestMethod]
    public async Task Given_ValidCreateProductCommand_When_HandleAsync_Then_ReturnsMappedCreateProductResult()
    {
        // Arrange (Given)
        var cancellationTokenSource = new CancellationTokenSource();
        var repository = new FakeProductRepository();
        var handler = new CreateProductCommandHandler(repository);
        var command = new CreateProductCommand
        {
            Name = "Arroz",
            Price = 1200m,
        };

        // Act (When)
        var result = await handler.HandleAsync(command, cancellationTokenSource.Token);

        // Assert (Then)
        Assert.AreEqual(1, repository.AddAsyncCallCount);
        Assert.AreEqual(command.Name, repository.LastAddedProduct!.Name);
        Assert.AreEqual(command.Price, repository.LastAddedProduct.Price);
        Assert.AreEqual(cancellationTokenSource.Token, repository.LastCancellationToken);
        Assert.AreEqual(repository.LastAddedProduct.Id, result.Id);
        Assert.AreEqual(command.Name, result.Name);
        Assert.AreEqual(command.Price, result.Price);
    }

    private sealed class FakeProductRepository : IProductRepository
    {
        public int AddAsyncCallCount { get; private set; }

        public Product? LastAddedProduct { get; private set; }

        public CancellationToken LastCancellationToken { get; private set; }

        public Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            AddAsyncCallCount++;
            LastAddedProduct = product;
            LastCancellationToken = cancellationToken;
            return Task.FromResult(product);
        }

        public Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
    }
}
