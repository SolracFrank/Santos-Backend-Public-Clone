namespace Domain.Exceptions;

public class BadRequestException(
    string message = "Bad request.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(400, "Bad request.", message, errorCode, innerException);
