namespace sabana.testing.service.tests.integration.infrastructure;

using Domain.Aggregates.Products;
using Infrastructure.Repositories;
using Infrastructure.data.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using sabana.testing.service;
using sabana.testing.service.tests.integration;

[TestClass]
[DoNotParallelize]
public sealed class ProductRepositoryIntegrationTests
{
    private SqliteConnection _connection = null!;
    private ApplicationDbContext _context = null!;
    private ProductRepository _repository = null!;

    [TestInitialize]
    public async Task SetUp()
    {
        _connection = SqliteInMemoryIntegrationTestConfig.CreateOpenedConnection();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new ApplicationDbContext(options);
        await SqliteInMemoryIntegrationTestConfig.EnsureCreatedAsync(_context);
        _repository = new ProductRepository(_context);
    }

    [TestCleanup]
    public void TearDown()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [TestMethod]
    public async Task Given_ValidDomainProduct_When_AddAsync_Then_ProductIsPersisted()
    {
        // Arrange (Given)
        var product = Product.Create("Arroz", 1200m);

        // Act (When)
        var savedProduct = await _repository.AddAsync(product);

        // Assert (Then)
        Assert.AreEqual(product.Id, savedProduct.Id);
        Assert.AreEqual(product.Name, savedProduct.Name);
        Assert.AreEqual(product.Price, savedProduct.Price);

        var storedEntity = await _context.ProductEntities.SingleAsync();
        Assert.AreEqual("Arroz", storedEntity.Name);
        Assert.AreEqual(1200m, storedEntity.Price);
    }

    [TestMethod]
    public async Task Given_PersistedProducts_When_GetAllAsync_Then_ReturnsMappedDomainProducts()
    {
        // Arrange (Given)
        _context.ProductEntities.AddRange(
            new ProductEntity { Id = Guid.NewGuid(), Name = "Frijol", Price = 900m },
            new ProductEntity { Id = Guid.NewGuid(), Name = "Lenteja", Price = 1100m });
        await _context.SaveChangesAsync();

        // Act (When)
        var products = await _repository.GetAllAsync();

        // Assert (Then)
        Assert.HasCount(2, products);
        Assert.IsTrue(products.Any(p => p.Name == "Frijol" && p.Price == 900m));
        Assert.IsTrue(products.Any(p => p.Name == "Lenteja" && p.Price == 1100m));
    }

    [TestMethod]
    public async Task Given_DuplicateProductName_When_AddAsync_Then_ThrowsDbUpdateException()
    {
        // Arrange (Given)
        await _repository.AddAsync(Product.Create("Arroz", 1200m));

        // Act (When)
        var act = () => _repository.AddAsync(Product.Create("Arroz", 900m));

        // Assert (Then)
        var exceptionThrown = false;

        try
        {
            await act();
            Assert.Fail("Se esperaba DbUpdateException por indice unico de nombre.");
        }
        catch (DbUpdateException)
        {
            exceptionThrown = true;
        }

        Assert.IsTrue(exceptionThrown);
    }
}
