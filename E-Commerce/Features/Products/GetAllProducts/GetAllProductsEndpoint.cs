using MediatR;

namespace E_Commerce.Features.Products.GetAllProducts;

public static class GetAllProductsEndpoint
{
    public static IEndpointRouteBuilder MapGetAllProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "GetAllProducts/{category?}",
                async (string? category, ISender sender) =>
                {
                    var query = new GetAllProductsQuery(category ?? "ALL");
                    var results = await sender.Send(query);
                    return TypedResults.Ok(results);
                }
            )
            .WithTags("GetAllProducts");
        return app;
    }
}
