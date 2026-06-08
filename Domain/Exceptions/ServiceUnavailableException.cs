namespace Domain.Exceptions;

public class ServiceUnavailableException(
    string message = "Service unavailable.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(503, "Service unavailable.", message, errorCode, innerException);
