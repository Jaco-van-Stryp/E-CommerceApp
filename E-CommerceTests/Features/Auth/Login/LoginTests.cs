using E_Commerce.Data;
using E_Commerce.Data.Entities;
using E_Commerce.Features.Auth.Login;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.JWTTokenService;
using E_Commerce.Services.PasswordService;
using E_CommerceTests.Factory;
using FluentAssertions;
using Moq;
using MockFactory = E_CommerceTests.Factory.MockFactory;

namespace E_CommerceTests.Features.Auth.Login;

public class LoginTests
{
    private static readonly string UserName = Guid.NewGuid().ToString();
    private static readonly string Password = Guid.NewGuid().ToString();
    private static readonly string JwtToken = Guid.NewGuid().ToString();
    private readonly AppDbContext _db = DbContextFactory.CreateDatabase();

    private readonly Mock<IJwtTokenService> _mockJwtTokenService = MockFactory.GetJwtService(
        token: JwtToken
    );

    private readonly IPasswordService _passwordService = new PasswordService();

    private async Task<Users> SeedUsers(string email, string password)
    {
        var hashedPassword = _passwordService.Hash(password);
        var user = new Users
        {
            Email = email,
            PasswordHash = hashedPassword.Hash,
            PasswordSalt = hashedPassword.Salt,
        };
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user;
    }

    //GivenWhenThen
    [Fact]
    public async Task Given_Login_When_ValidCredentials_Then_GenerateValidToken()
    {
        await SeedUsers(email: UserName, Password);
        var command = new LoginCommand(Email: UserName, Password: Password);
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        var results = await handler.Handle(command, CancellationToken.None);
        results.Token.Should().Be(JwtToken);
    }

    [Fact]
    public async Task Given_Login_When_InvalidCredentials_Then_ThrowUnauthorizedException()
    {
        await SeedUsers(email: UserName, Password);
        var command = new LoginCommand(Email: UserName, Guid.NewGuid().ToString());
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Given_Login_When_UserNotFound_Then_ThrowUnauthorizedException()
    {
        await SeedUsers(email: UserName, Password);
        var command = new LoginCommand(
            Email: "fake-user@test.com",
            Password: Guid.NewGuid().ToString()
        );
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Given_Login_When_UserNotFoundButValidPassword_Then_ThrowUnauthorizedException()
    {
        await SeedUsers(email: UserName, Password);
        var command = new LoginCommand(Email: "fake-user@test.com", Password: Password);
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
