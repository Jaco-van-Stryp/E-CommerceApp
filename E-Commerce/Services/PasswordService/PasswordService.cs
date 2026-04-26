using System.Security.Cryptography;
using System.Text;

namespace E_Commerce.Services.PasswordService;

public class PasswordService : IPasswordService
{
    public HashedPassword Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = ComputeHash(password, salt);
        return new HashedPassword(Convert.ToBase64String(hash), Convert.ToBase64String(salt));
    }

    public bool Verify(string password, string storedHash, string storedSalt)
    {
        var salt = Convert.FromBase64String(storedSalt);
        var computed = ComputeHash(password, salt);
        var stored = Convert.FromBase64String(storedHash);
        return CryptographicOperations.FixedTimeEquals(computed, stored);
    }


private static byte[] ComputeHash(string password, byte[] salt) =>
        Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            350_000,
            HashAlgorithmName.SHA512,
            64
        );
}
