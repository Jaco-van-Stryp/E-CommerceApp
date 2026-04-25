namespace E_Commerce.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("auth").WithTags("Auth");
        return app;
    }
}
