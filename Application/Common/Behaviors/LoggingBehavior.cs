using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var started = Stopwatch.GetTimestamp();
        var requestName = typeof(TRequest).Name;
        using var scope = logger.BeginScope("Request: {RequestName}", requestName);

        var response = await next(cancellationToken);
        var elapsed = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
        if (elapsed >= 1_000)
            logger.LogWarning("Slow request {RequestName} took {ElapsedMilliseconds} ms.", requestName, elapsed);
        else
            logger.LogDebug("Handled {RequestName} in {ElapsedMilliseconds} ms.", requestName, elapsed);

        return response;
    }
}
