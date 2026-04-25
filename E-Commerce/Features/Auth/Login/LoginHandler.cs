using E_Commerce.Data;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.JWTTokenService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Features.Auth.Login;

public class LoginHandler(AppDbContext dbContext, IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(
            x => x.Email == request.Email,
            cancellationToken
        );
        if (user == null)
        {
            Log.Warning("Unauthorized login request by {Email}", request.Email);
            throw new UnauthorizedException();
        }
        var token = jwtTokenService.GenerateToken(userId: user.Id, email: user.Email);
        Log.Information("User {UserEmail} successfully logged in.", user.Email);
        return new LoginResponse { Token = token };
    }
}
