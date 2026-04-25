using MediatR;
using Microsoft.AspNetCore.Identity.Data;

namespace E_Commerce.Features.Auth.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "Login",
                (ISender sender, LoginRequest request) =>
                {
                    sender.Send(request);
                }
            )
            .WithName("Login");
        return app;
    }
}
