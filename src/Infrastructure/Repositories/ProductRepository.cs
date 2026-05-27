using Domain.Aggregates.Products;
using Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using sabana.testing.service;

namespace Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        var entity = ProductMapper.ToDataEntity(product);

        await _context.ProductEntities.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return ProductMapper.ToDomainEntity(entity);
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.ProductEntities
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities.Select(ProductMapper.ToDomainEntity).ToList();
    }
}