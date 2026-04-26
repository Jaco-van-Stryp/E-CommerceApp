namespace E_Commerce.Services.JWTTokenService
{
    public class JwtOptions
    {
        public required string Secret { get; set; }

        public string Issuer { get; set; } = "E-Commerce";

        public string Audience { get; set; } = "E-CommerceClient";

        public int ExpirationMinutes { get; set; } = 480; // 8 hours; override via Jwt:ExpirationMinutes in appsettings
    }
}
