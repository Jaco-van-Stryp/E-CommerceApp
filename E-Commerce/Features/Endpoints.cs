using E_Commerce.Features.Auth;

namespace E_Commerce.Features;

public static class Endpoints
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api");
        group.MapAuthEndpoints();
        return app;
    }
}
