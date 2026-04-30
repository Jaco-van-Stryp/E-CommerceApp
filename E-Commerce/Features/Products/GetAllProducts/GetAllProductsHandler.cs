using E_Commerce.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Features.Products.GetAllProducts;

public class GetAllProductsHandler(AppDbContext database)
    : IRequestHandler<GetAllProductsQuery, List<GetAllProductsResults>>
{
    public async Task<List<GetAllProductsResults>> Handle(
        GetAllProductsQuery request,
        CancellationToken cancellationToken
    )
    {
        const string filePreFix = "TODO\\";
        bool isAllCategories = request.Category == null;

        var products = await database
            .Products.Where(x => isAllCategories || x.ProductCategory == request.Category)
            .ToListAsync(cancellationToken);

        return products
            .Select(product => new GetAllProductsResults(
                Id: product.Id,
                ProductName: product.ProductName,
                ProductDescription: product.ProductDescription,
                ProductPrice: product.ProductPrice,
                ProductImageFileName: filePreFix + product.ProductImageFileName,
                ProductCategory: product.ProductCategory
            ))
            .ToList();
    }
}
