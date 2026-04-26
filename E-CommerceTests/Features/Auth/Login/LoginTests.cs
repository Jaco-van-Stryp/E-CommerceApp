using E_Commerce.Data;
using E_Commerce.Data.Entities;
using E_Commerce.Features.Auth.Login;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.CurrentUserService;
using E_Commerce.Services.JWTTokenService;
using E_Commerce.Services.PasswordService;
using E_CommerceTests.Factory;
using FluentAssertions;
using Moq;
using MockFactory = E_CommerceTests.Factory.MockFactory;

namespace E_CommerceTests;

public class LoginTests
{
    private static readonly string _userName = Guid.NewGuid().ToString();
    private static readonly string _password = Guid.NewGuid().ToString();
    private static readonly Guid _userId = Guid.NewGuid();
    private static readonly string _jwtToken = Guid.NewGuid().ToString();
    private readonly AppDbContext _db = DbContextFactory.CreateDatabase();
    private readonly Mock<ICurrentUserService> _mockCurrentUserService =
        MockFactory.GetCurrentUserService(currentUser: "shop@user.com", currentUserId: _userId);
    private readonly Mock<IJwtTokenService> _mockJwtTokenService = MockFactory.GetJwtService(
        token: _jwtToken
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
        await SeedUsers(email: _userName, _password);
        var command = new LoginCommand(Email: _userName, Password: _password);
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        var results = await handler.Handle(command, CancellationToken.None);
        results.Token.Should().Be(_jwtToken);
    }

    [Fact]
    public async Task Given_Login_When_InvalidCredentials_Then_ThrowUnauthorizedException()
    {
        await SeedUsers(email: _userName, _password);
        var command = new LoginCommand(Email: _userName, Guid.NewGuid().ToString());
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Given_Login_When_UserNotFound_Then_ThrowUnauthorizedException()
    {
        await SeedUsers(email: _userName, _password);
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
        await SeedUsers(email: _userName, _password);
        var command = new LoginCommand(Email: "fake-user@test.com", Password: _password);
        var handler = new LoginHandler(_db, _mockJwtTokenService.Object, _passwordService);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
