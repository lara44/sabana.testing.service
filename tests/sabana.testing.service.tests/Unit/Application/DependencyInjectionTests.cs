namespace sabana.testing.service.tests.unit.application;

using Application;
using Microsoft.Extensions.DependencyInjection;

[TestClass]
public sealed class DependencyInjectionTests
{
    [TestMethod]
    public void Given_ServiceCollection_When_AddApplication_Then_ReturnsSameServiceCollection()
    {
        // Arrange (Given)
        IServiceCollection services = new ServiceCollection();

        // Act (When)
        var returnedServices = services.AddApplication();

        // Assert (Then)
        Assert.AreSame(services, returnedServices);
    }
}
