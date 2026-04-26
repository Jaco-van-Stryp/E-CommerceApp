using E_Commerce.Services.CurrentUserService;
using E_Commerce.Services.JWTTokenService;
using E_Commerce.Services.PasswordService;
using Moq;

namespace E_CommerceTests.Factory;

public static class MockFactory
{
    public static Mock<IJwtTokenService> GetJwtService(string token)
    {
        var mockJwtService = new Mock<IJwtTokenService>();
        mockJwtService
            .Setup(service =>
                service.GenerateToken(userId: It.IsAny<Guid>(), email: It.IsAny<string>())
            )
            .Returns(token);
        return mockJwtService;
    }

    public static Mock<ICurrentUserService> GetCurrentUserService(
        string currentUser,
        Guid currentUserId
    )
    {
        var mockCurrentUserService = new Mock<ICurrentUserService>();
        mockCurrentUserService.Setup(service => service.Email).Returns(currentUser);
        mockCurrentUserService.Setup(service => service.UserId).Returns(currentUserId);
        return mockCurrentUserService;
    }
}
