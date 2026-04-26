using E_Commerce.Features.Auth;
using E_Commerce.Features.Products;

namespace E_Commerce.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api");
        group.MapAuthEndpoints();
        group.MapProductEndpoints();
        return app;
    }
}
