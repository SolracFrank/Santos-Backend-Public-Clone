namespace Domain.Exceptions;

public class ForbiddenException(
    string message = "Access to this resource is forbidden.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(403, "Forbidden.", message, errorCode, innerException);
