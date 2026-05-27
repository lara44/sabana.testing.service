using Domain.Aggregates.Products;
using Infrastructure.data.Entities;

namespace Infrastructure.Mappers;

public static class ProductMapper
{
    public static ProductEntity ToDataEntity(Product domain)
    {
        return new ProductEntity
        {
            Id = domain.Id,
            Name = domain.Name,
            Price = domain.Price
        };
    }

    public static Product ToDomainEntity(ProductEntity data)
    {
        return Product.Rehydrate(data.Id, data.Name, data.Price);
    }
}