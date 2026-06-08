namespace Domain.Exceptions;

public class UnauthenticatedException(
    string message = "Authentication is required.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(401, "Unauthenticated.", message, errorCode, innerException);
