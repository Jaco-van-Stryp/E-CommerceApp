using E_Commerce.Data;
using MediatR;

namespace E_Commerce.Features.Products.CreateProduct;

public class CreateProductHandler(AppDbContext database)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    public async Task<CreateProductResponse> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken
    )
    {
        // No need to care about duplicate Products, since we assume for now they can have the same names.
        var newProduct = new Data.Entities.Products
        {
            ProductName = request.ProductName,
            ProductDescription = request.ProductDescription,
            ProductPrice = request.ProductPrice,
            ProductCategory = request.ProductCategory,
            ProductImageFileName = request.ProductImage,
        };

        await database.Products.AddAsync(newProduct, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        //TODO Set product Prefix with service.
        var productImageFileNameWithPrefix = "TODO\\" + request.ProductImage;

        var productResponse = new CreateProductResponse(
            Id: newProduct.Id,
            ProductName: newProduct.ProductName,
            ProductDescription: newProduct.ProductDescription,
            ProductPrice: newProduct.ProductPrice,
            ProductImageFileName: productImageFileNameWithPrefix,
            ProductCategory: newProduct.ProductCategory
        );
        return productResponse;
    }
}
