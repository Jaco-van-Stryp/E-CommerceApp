using E_Commerce.Data;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.JWTTokenService;
using E_Commerce.Services.PasswordService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Features.Auth.Login;

public class LoginHandler(
    AppDbContext dbContext,
    IJwtTokenService jwtTokenService,
    IPasswordService passwordService
) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(
            x => x.Email == request.Email.ToLowerInvariant(),
            cancellationToken
        );

        if (user == null || !passwordService.Verify(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            Log.Warning("Unauthorized login request by {Email}", request.Email);
            throw new UnauthorizedException();
        }
        var token = jwtTokenService.GenerateToken(userId: user.Id, email: user.Email);
        Log.Information("User {UserEmail} successfully logged in.", user.Email);
        return new LoginResponse { Token = token };
    }
}
