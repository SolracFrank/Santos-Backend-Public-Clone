namespace Domain.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(
        int statusCode,
        string title,
        string message,
        string? errorCode = null,
        Exception? innerException = null) : base(message, innerException)
    {
        StatusCode = statusCode;
        Title = title;
        ErrorCode = errorCode;
    }

    public int StatusCode { get; }
    public string Title { get; }
    public string? ErrorCode { get; }
}
