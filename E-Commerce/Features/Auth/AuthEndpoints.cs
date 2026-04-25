using E_Commerce.Features.Auth.Login;
using E_Commerce.Features.Auth.Register;

namespace E_Commerce.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("auth").WithTags("Auth");
        group.MapLoginEndpoint();
        group.MapRegisterEndpoint();
        return app;
    }
}
