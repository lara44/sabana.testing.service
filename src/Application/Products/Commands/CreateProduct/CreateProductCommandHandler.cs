using AtcMediator;
using Domain.Aggregates.Products;

namespace Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IAtcRequestHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<CreateProductResult> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = Product.Create(command.Name, command.Price);
        var savedProduct = await _productRepository.AddAsync(product, cancellationToken);

        return new CreateProductResult
        {
            Id = savedProduct.Id,
            Name = savedProduct.Name,
            Price = savedProduct.Price
        };
    }
}
