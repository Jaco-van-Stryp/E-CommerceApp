using E_Commerce.Data;
using E_Commerce.Features.Auth.Register;
using E_Commerce.Infrastructure.Exceptions;
using E_Commerce.Services.JWTTokenService;
using E_Commerce.Services.PasswordService;
using E_CommerceTests.Factory;
using FluentAssertions;
using Moq;
using MockFactory = E_CommerceTests.Factory.MockFactory;

namespace E_CommerceTests.Features.Auth.Register;

public class RegisterTests
{
    private static readonly string EmailAddress = "TestUser@gmail.com";
    private static readonly string JwtToken = Guid.NewGuid().ToString();
    private readonly AppDbContext _db = DbContextFactory.CreateDatabase();

    private readonly Mock<IJwtTokenService> _mockJwtTokenService = MockFactory.GetJwtService(
        token: JwtToken
    );

    private readonly IPasswordService _passwordService = new PasswordService();

    [Fact]
    public async Task Given_RegisterUser_When_NewUser_Then_CreateUser()
    {
        var command = new RegisterCommand(Email: EmailAddress, Password: Guid.NewGuid().ToString());
        var handler = new RegisterHandler(_db, _mockJwtTokenService.Object, _passwordService);
        var results = await handler.Handle(command, CancellationToken.None);
        results.Token.Should().Be(JwtToken);
    }

    [Fact]
    public async Task Given_RegisterUser_When_RegisterExistingUser_Then_ThrowConflictException()
    {
        var command = new RegisterCommand(Email: EmailAddress, Password: Guid.NewGuid().ToString());
        var handler = new RegisterHandler(_db, _mockJwtTokenService.Object, _passwordService);
        // Send two requests of the same data, with the same database.
        var results = await handler.Handle(command, CancellationToken.None);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task Given_RegisterUser_When_InvalidEmail_Then_ThrowInvalidEmailException()
    {
        var email = "thisIsNotAnEmail";
        var password = Guid.NewGuid().ToString();

        var command = new RegisterCommand(Email: email, Password: password);
        var handler = new RegisterHandler(_db, _mockJwtTokenService.Object, _passwordService);
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidEmailException>();
    }
}
