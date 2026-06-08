namespace Domain.Exceptions;

public class NotFoundException(
    string message = "Resource not found.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(404, "Resource not found.", message, errorCode, innerException);
