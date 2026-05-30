namespace sabana.testing.service.tests.unit.infrastructure.mappers;

using Domain.Aggregates.Products;
using Infrastructure.Mappers;
using Infrastructure.data.Entities;

[TestClass]
public sealed class ProductMapperTests
{
    [TestMethod]
    public void Given_DomainProduct_When_ToDataEntity_Then_ReturnsMappedEntity()
    {
        // Arrange (Given)
        var product = Product.Rehydrate(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Arroz", 1200m);

        // Act (When)
        var entity = ProductMapper.ToDataEntity(product);

        // Assert (Then)
        Assert.AreEqual(product.Id, entity.Id);
        Assert.AreEqual(product.Name, entity.Name);
        Assert.AreEqual(product.Price, entity.Price);
    }

    [TestMethod]
    public void Given_DataProductEntity_When_ToDomainEntity_Then_ReturnsMappedProduct()
    {
        // Arrange (Given)
        var entity = new ProductEntity
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Frijol",
            Price = 900m,
        };

        // Act (When)
        var product = ProductMapper.ToDomainEntity(entity);

        // Assert (Then)
        Assert.AreEqual(entity.Id, product.Id);
        Assert.AreEqual(entity.Name, product.Name);
        Assert.AreEqual(entity.Price, product.Price);
    }
}
