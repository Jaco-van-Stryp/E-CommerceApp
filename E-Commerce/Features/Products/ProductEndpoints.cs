using E_Commerce.Features.Products.CreateProduct;

namespace E_Commerce.Features.Products;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        //TODO - Admin users should be able to create products, for now, free for all during testing
        var group = app.MapGroup("Products").WithTags("Products").RequireAuthorization();
        group.MapCreateProductEndpoint();
        return group;
    }
}
