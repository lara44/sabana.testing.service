namespace sabana.testing.service.tests.unit.domain;

using Domain.Aggregates.Products;

[TestClass]
public sealed class ProductAggregateTests
{
    [TestMethod]
    public void Given_EmptyName_When_Create_Then_ThrowsArgumentException()
    {
        // Arrange (Given)
        var name = string.Empty;
        var price = 1200m;

        // Act (When)
        Action act = () => Product.Create(name, price);

        // Assert (Then)
        Assert.Throws<ArgumentException>(act);
    }

    [TestMethod]
    public void Given_NonPositivePrice_When_Create_Then_ThrowsArgumentException()
    {
        // Arrange (Given)
        var name = "Arroz";
        var price = 0m;

        // Act (When)
        Action act = () => Product.Create(name, price);

        // Assert (Then)
        Assert.Throws<ArgumentException>(act);
    }
}
