using MediatR;

namespace E_Commerce.Features.Auth.Login;

public static class LoginEndpoint
{
    public static IEndpointRouteBuilder MapLoginEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "Login",
                async (ISender sender, LoginCommand request) =>
                {
                    var response = await sender.Send(request);
                    return TypedResults.Ok(response);
                }
            )
            .WithName("Login");
        return app;
    }
}
