namespace E_Commerce.Features.Products.GetAllProducts;

public readonly record struct GetAllProductsResults(
    Guid Id,
    string ProductName,
    string ProductDescription,
    decimal ProductPrice,
    string ProductImageFileName,
    string ProductCategory
);
