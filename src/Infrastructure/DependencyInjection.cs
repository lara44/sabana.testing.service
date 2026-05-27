using Domain.Aggregates.Products;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace sabana.testing.service;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                }));

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }

}
