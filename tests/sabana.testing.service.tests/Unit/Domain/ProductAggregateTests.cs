namespace sabana.testing.service.tests.unit.domain;

using Domain.Aggregates.Products;

[TestClass]
public sealed class ProductAggregateTests
{
    [TestMethod]
    public void Create_ShouldThrowArgumentException_WhenNameIsEmpty()
    {
        Assert.Throws<ArgumentException>(() => Product.Create(string.Empty, 1200m));
    }

    [TestMethod]
    public void Create_ShouldThrowArgumentException_WhenPriceIsNotPositive()
    {
        Assert.Throws<ArgumentException>(() => Product.Create("Arroz", 0m));
    }
}
