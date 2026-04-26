using MediatR;

namespace E_Commerce.Features.Products.CreateProduct;

public record CreateProductCommand(
    string ProductName,
    string ProductDescription,
    decimal ProductPrice,
    string ProductCategory,
    Guid ProductImage
) : IRequest<CreateProductResponse>;
