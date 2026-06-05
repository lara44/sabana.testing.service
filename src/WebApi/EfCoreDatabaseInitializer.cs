using Microsoft.EntityFrameworkCore;
using sabana.testing.service;

namespace WebApi;

public sealed class EfCoreDatabaseInitializer : IDatabaseInitializer
{
    private readonly ApplicationDbContext _dbContext;

    public EfCoreDatabaseInitializer(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Database.MigrateAsync(cancellationToken);
    }
}