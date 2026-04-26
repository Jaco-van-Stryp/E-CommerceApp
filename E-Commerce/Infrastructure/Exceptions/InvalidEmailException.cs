using E_Commerce.Services.ExceptionHandling;

namespace E_Commerce.Infrastructure.Exceptions;

public class InvalidEmailException()
    : ApiException("The email you have entered is invalid", StatusCodes.Status406NotAcceptable);
