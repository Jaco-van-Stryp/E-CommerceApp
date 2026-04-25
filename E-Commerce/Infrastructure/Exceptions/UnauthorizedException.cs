using E_Commerce.Services.ExceptionHandling;

namespace E_Commerce.Infrastructure.Exceptions;

public class UnauthorizedException()
    : ApiException(
        "You are not authorized to perform this action",
        StatusCodes.Status401Unauthorized
    );
