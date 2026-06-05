namespace sabana.testing.service.tests.integration.webapi;

using System.Net;
using System.Net.Http.Json;
using Application.Products.Commands.CreateProduct;
using Application.Products.Queries.GetProducts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Common;
using sabana.testing.service;
using sabana.testing.service.tests.integration;

[TestClass]
[DoNotParallelize]
public sealed class ProductEndpointsIntegrationTests
{
    [TestMethod]
    public async Task Given_NoProducts_When_GetProducts_Then_ReturnsOkWithEmptyCollection()
    {
        // Arrange (Given)
        using var factory = new TestingWebApplicationFactory();
        using var client = factory.CreateClient();

        // Act (When)
        var response = await client.GetAsync("/api/products");

        // Assert (Then)
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<ResponseDto<IReadOnlyCollection<GetProductsResult>>>();
        Assert.IsNotNull(payload);
        Assert.IsFalse(payload.Error);
        Assert.IsNotNull(payload.Data);
        Assert.IsEmpty(payload.Data);
    }

    [TestMethod]
    public async Task Given_ValidRequest_When_PostProduct_Then_ReturnsCreatedResponse()
    {
        // Arrange (Given)
        using var factory = new TestingWebApplicationFactory();
        using var client = factory.CreateClient();
        var request = new CreateProductCommand
        {
            Name = "Arroz",
            Price = 1200m,
        };

        // Act (When)
        var response = await client.PostAsJsonAsync("/api/products", request);

        // Assert (Then)
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<ResponseDto<CreateProductResult>>();
        Assert.IsNotNull(payload);
        Assert.IsFalse(payload.Error);
        Assert.AreEqual(201, payload.Code);
        Assert.IsNotNull(payload.Data);
        Assert.AreEqual("Arroz", payload.Data.Name);
        Assert.AreEqual(1200m, payload.Data.Price);
        Assert.AreNotEqual(Guid.Empty, payload.Data.Id);
    }

    [TestMethod]
    public async Task Given_InvalidRequest_When_PostProduct_Then_ReturnsBadRequest()
    {
        // Arrange (Given)
        using var factory = new TestingWebApplicationFactory();
        using var client = factory.CreateClient();
        var request = new CreateProductCommand
        {
            Name = string.Empty,
            Price = 100m,
        };

        // Act (When)
        var response = await client.PostAsJsonAsync("/api/products", request);

        // Assert (Then)
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<ResponseDto<object>>();
        Assert.IsNotNull(payload);
        Assert.IsTrue(payload.Error);
        Assert.AreEqual(400, payload.Code);
    }

    [TestMethod]
    public async Task Given_ProductCreated_When_GetProducts_Then_ReturnsCollectionWithCreatedProduct()
    {
        // Arrange (Given)
        using var factory = new TestingWebApplicationFactory();
        using var client = factory.CreateClient();

        await client.PostAsJsonAsync("/api/products", new CreateProductCommand
        {
            Name = "Frijol",
            Price = 900m,
        });

        // Act (When)
        var response = await client.GetAsync("/api/products");

        // Assert (Then)
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<ResponseDto<IReadOnlyCollection<GetProductsResult>>>();
        Assert.IsNotNull(payload);
        Assert.IsNotNull(payload.Data);
        Assert.HasCount(1, payload.Data);

        var product = payload.Data.Single();
        Assert.AreEqual("Frijol", product.Name);
        Assert.AreEqual(900m, product.Price);
    }

    private sealed class TestingWebApplicationFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection? _connection;

        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.UseSetting("ASPNETCORE_ENVIRONMENT", "Testing");

            builder.ConfigureServices(services =>
            {
                _connection = SqliteInMemoryIntegrationTestConfig.CreateOpenedConnection();
                SqliteInMemoryIntegrationTestConfig.ReplaceApplicationDbContextWithSqlite(services, _connection);

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SqliteInMemoryIntegrationTestConfig.EnsureCreatedAsync(db).GetAwaiter().GetResult();
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _connection?.Dispose();
        }
    }
}
