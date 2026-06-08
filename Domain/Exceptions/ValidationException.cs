namespace Domain.Exceptions;

public class ValidationException(
    string message = "Validation failed.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(400, "Validation failed.", message, errorCode, innerException);
