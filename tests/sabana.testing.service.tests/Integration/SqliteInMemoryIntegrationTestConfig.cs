namespace sabana.testing.service.tests.integration;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using sabana.testing.service;
using WebApi;

internal static class SqliteInMemoryIntegrationTestConfig
{
    private const string ConnectionString = "Data Source=:memory:";

    internal static SqliteConnection CreateOpenedConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }

    internal static void ReplaceApplicationDbContextWithSqlite(IServiceCollection services, SqliteConnection connection)
    {
        services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
        services.RemoveAll<ApplicationDbContext>();
        services.RemoveAll<IDbContextOptionsConfiguration<ApplicationDbContext>>();
        services.RemoveAll<IDatabaseInitializer>();

        services.AddSingleton(connection);
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connection));
        services.AddScoped<IDatabaseInitializer, NoOpDatabaseInitializer>();
    }

    internal static async Task EnsureCreatedAsync(ApplicationDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    private sealed class NoOpDatabaseInitializer : IDatabaseInitializer
    {
        public Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            _ = cancellationToken;
            return Task.CompletedTask;
        }
    }
}