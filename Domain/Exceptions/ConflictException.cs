namespace Domain.Exceptions;

public class ConflictException(
    string message = "Conflict.",
    string? errorCode = null,
    Exception? innerException = null)
    : ApiException(409, "Conflict.", message, errorCode, innerException);
