namespace E_Commerce.Services.JWTTokenService
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string email);
    }
}
