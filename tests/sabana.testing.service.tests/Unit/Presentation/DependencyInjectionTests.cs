namespace sabana.testing.service.tests.unit.presentation;

using Microsoft.Extensions.DependencyInjection;
using Presentation;

[TestClass]
public sealed class DependencyInjectionTests
{
    [TestMethod]
    public void Given_ServiceCollection_When_AddPresentation_Then_ReturnsSameServiceCollection()
    {
        // Arrange (Given)
        IServiceCollection services = new ServiceCollection();

        // Act (When)
        var returnedServices = services.AddPresentation();

        // Assert (Then)
        Assert.AreSame(services, returnedServices);
    }
}
