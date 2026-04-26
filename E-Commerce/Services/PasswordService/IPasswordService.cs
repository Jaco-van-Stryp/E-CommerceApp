namespace E_Commerce.Services.PasswordService;

public record HashedPassword(string Hash, string Salt);

public interface IPasswordService
{
    HashedPassword Hash(string password);
    bool Verify(string password, string storedHash, string storedSalt);
}
