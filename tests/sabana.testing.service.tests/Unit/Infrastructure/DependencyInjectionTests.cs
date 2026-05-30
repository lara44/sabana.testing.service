namespace sabana.testing.service.tests.unit.infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sabana.testing.service;

[TestClass]
public sealed class DependencyInjectionTests
{
    [TestMethod]
    public void Given_ServiceCollection_When_AddInfrastructure_Then_ReturnsSameServiceCollection()
    {
        // Arrange (Given)
        IServiceCollection services = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=tests;Username=tests;Password=tests",
            })
            .Build();

        // Act (When)
        var returnedServices = services.AddInfrastructure(configuration);

        // Assert (Then)
        Assert.AreSame(services, returnedServices);
    }
}
