using MediatR;

namespace E_Commerce.Features.Auth.Register;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "Register",
                async (ISender sender, RegisterCommand request) =>
                {
                    var response = await sender.Send(request);
                    return Results.Ok(response);
                }
            )
            .WithName("Register");
        return app;
    }
}
