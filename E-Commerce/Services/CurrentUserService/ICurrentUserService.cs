namespace E_Commerce.Services.CurrentUserService;

public interface ICurrentUserService
{
    string Email { get; }
    Guid UserId { get; }
}
