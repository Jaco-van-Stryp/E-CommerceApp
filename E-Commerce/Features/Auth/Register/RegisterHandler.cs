using System.Security.Cryptography;
using System.Text;
using E_Commerce.Data;
using E_Commerce.Data.Entities;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.JWTTokenService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Features.Auth.Register;

public class RegisterHandler(AppDbContext dbContext, IJwtTokenService jwtTokenService)
    : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var emailTaken = await dbContext.Users.AnyAsync(
            x => x.Email == request.Email,
            cancellationToken
        );
        if (emailTaken)
            throw new ConflictException($"A user with email '{request.Email}' already exists.");

        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(request.Password),
            salt,
            350_000,
            HashAlgorithmName.SHA512,
            64
        );

        var user = new Users
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            PasswordHash = Convert.ToBase64String(hash),
            PasswordSalt = Convert.ToBase64String(salt),
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var token = jwtTokenService.GenerateToken(userId: user.Id, email: user.Email);
        Log.Information("User {UserEmail} successfully registered.", user.Email);
        return new RegisterResponse { Token = token };
    }
}
