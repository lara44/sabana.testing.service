namespace sabana.testing.service.tests.unit.infrastructure.repositories;

using Domain.Aggregates.Products;
using Infrastructure.Repositories;
using Infrastructure.data.Entities;
using Microsoft.EntityFrameworkCore;
using sabana.testing.service;

[TestClass]
public sealed class ProductRepositoryTests
{
    [TestMethod]
    public async Task Given_NewProduct_When_AddAsync_Then_PersistsAndReturnsMappedProduct()
    {
        // Arrange (Given)
        using var context = CreateContext();
        var repository = new ProductRepository(context);
        var product = Product.Create("Arroz", 1200m);

        // Act (When)
        var savedProduct = await repository.AddAsync(product);

        // Assert (Then)
        Assert.AreEqual(product.Id, savedProduct.Id);
        Assert.AreEqual(product.Name, savedProduct.Name);
        Assert.AreEqual(product.Price, savedProduct.Price);
        var storedEntitiesCount = await context.ProductEntities.CountAsync();
        Assert.AreEqual(1, storedEntitiesCount);

        var storedEntity = await context.ProductEntities.SingleAsync();
        Assert.AreEqual(product.Id, storedEntity.Id);
        Assert.AreEqual(product.Name, storedEntity.Name);
        Assert.AreEqual(product.Price, storedEntity.Price);
    }

    [TestMethod]
    public async Task Given_PersistedProducts_When_GetAllAsync_Then_ReturnsMappedProducts()
    {
        // Arrange (Given)
        using var context = CreateContext();
        context.ProductEntities.AddRange(
            new ProductEntity
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Arroz",
                Price = 1200m,
            },
            new ProductEntity
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Frijol",
                Price = 900m,
            });
        await context.SaveChangesAsync();

        var repository = new ProductRepository(context);

        // Act (When)
        var products = await repository.GetAllAsync();

        // Assert (Then)
        Assert.HasCount(2, products);

        var firstProduct = products.ElementAt(0);
        var secondProduct = products.ElementAt(1);

        Assert.AreEqual(Guid.Parse("11111111-1111-1111-1111-111111111111"), firstProduct.Id);
        Assert.AreEqual("Arroz", firstProduct.Name);
        Assert.AreEqual(1200m, firstProduct.Price);
        Assert.AreEqual(Guid.Parse("22222222-2222-2222-2222-222222222222"), secondProduct.Id);
        Assert.AreEqual("Frijol", secondProduct.Name);
        Assert.AreEqual(900m, secondProduct.Price);
    }

    private static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }
}
