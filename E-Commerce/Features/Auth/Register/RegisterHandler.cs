using E_Commerce.Data;
using E_Commerce.Data.Entities;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.JWTTokenService;
using E_Commerce.Services.PasswordService;
using EmailValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace E_Commerce.Features.Auth.Register;

public class RegisterHandler(
    AppDbContext dbContext,
    IJwtTokenService jwtTokenService,
    IPasswordService passwordService
) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var email = request.Email.ToLowerInvariant();

        if (!EmailValidator.Validate(email))
            throw new InvalidEmailException();

        var emailTaken = await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
        if (emailTaken)
            throw new ConflictException($"A user with email '{email}' already exists.");

        var hashed = passwordService.Hash(request.Password);

        var user = new Users
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = hashed.Hash,
            PasswordSalt = hashed.Salt,
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        var token = jwtTokenService.GenerateToken(userId: user.Id, email: user.Email);
        Log.Information("User {UserEmail} successfully registered.", user.Email);
        return new RegisterResponse { Token = token };
    }
}
