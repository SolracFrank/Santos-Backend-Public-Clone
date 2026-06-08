namespace Domain.Exceptions;

public class UnauthorizedException(
    string message = "You are not authorized to perform this action.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(403, "Unauthorized.", message, errorCode, innerException);
