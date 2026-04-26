using MediatR;

namespace E_Commerce.Features.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static Task<IEndpointRouteBuilder> MapCreateProductEndpoint(
        this IEndpointRouteBuilder app
    )
    {
        app.MapPost(
                "CreateProduct",
                async (ISender sender, CreateProductCommand command) =>
                {
                    var results = await sender.Send(command);
                    return TypedResults.Ok(results);
                }
            )
            .WithTags("CreateProduct")
            .RequireAuthorization();
        return Task.FromResult(app);
    }
}
