using MediatR;

namespace E_Commerce.Features.Auth.Register;

public readonly record struct RegisterCommand(string Email, string Password) : IRequest<RegisterResponse>;
