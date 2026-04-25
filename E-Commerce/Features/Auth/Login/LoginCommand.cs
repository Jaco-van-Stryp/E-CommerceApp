using MediatR;

namespace E_Commerce.Features.Auth.Login;

public readonly record struct LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
