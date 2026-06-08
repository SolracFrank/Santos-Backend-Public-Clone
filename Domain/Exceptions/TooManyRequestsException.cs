namespace Domain.Exceptions;

public class TooManyRequestsException(
    string message = "Too many requests.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(429, "Too many requests.", message, errorCode, innerException);
