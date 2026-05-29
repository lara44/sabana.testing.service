using Domain.Abstractions;

namespace Domain.Aggregates.Products;

public class Product : AggregateRoot<Guid>
{
    public string Name { get; private set; } = null!;
    public decimal Price { get; private set; }

    private Product() { }

    private Product(Guid id, string name, decimal price) : base(id)
    {
        Name = name;
        Price = price;
    }

    public static Product Create(string name, decimal price)
    {
        ValidateName(name);
        ValidatePrice(price);

        return new Product(Guid.NewGuid(), name, price);
    }

    public static Product Rehydrate(Guid id, string name, decimal price)
    {
        return new Product(id, name, price);
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("El nombre del producto es obligatorio.", nameof(name));
        }
    }

    private static void ValidatePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new ArgumentException("El precio del producto debe ser mayor que cero.", nameof(price));
        }
    }
}