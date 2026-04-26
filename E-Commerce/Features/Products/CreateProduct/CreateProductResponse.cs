namespace E_Commerce.Features.Products.CreateProduct;

public readonly record struct CreateProductResponse(
    Guid Id,
    string ProductName,
    string ProductDescription,
    decimal ProductPrice,
    string ProductImageFileName,
    string ProductCategory
);
