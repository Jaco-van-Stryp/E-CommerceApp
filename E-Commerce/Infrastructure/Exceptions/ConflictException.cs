using E_Commerce.Services.ExceptionHandling;

namespace E_Commerce.Infrastructure.Exceptions;

public class ConflictException(string message)
    : ApiException(message, StatusCodes.Status409Conflict);
