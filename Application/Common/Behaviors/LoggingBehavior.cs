using System.Diagnostics;
using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const long SlowRequestThresholdMilliseconds = 1_000;
    private static readonly HashSet<string> SensitivePropertyNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "Password",
        "Token",
        "AccessToken",
        "RefreshToken",
        "Secret",
        "ApiKey",
        "Session"
    };

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var featureName = typeof(TRequest).Namespace?.Split('.').LastOrDefault() ?? "Unknown";
        var requestData = GetSafeRequestData(request);
        var stopwatch = Stopwatch.StartNew();

        using var scope = _logger.BeginScope(new Dictionary<string, object?>
        {
            ["Feature"] = featureName,
            ["RequestName"] = requestName
        });

        _logger.LogInformation(
            "Handling {RequestName} for feature {Feature}. RequestData: {@RequestData}",
            requestName,
            featureName,
            requestData);

        try
        {
            var response = await next(cancellationToken);
            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds >= SlowRequestThresholdMilliseconds)
            {
                _logger.LogWarning(
                    "Handled slow request {RequestName} for feature {Feature} in {ElapsedMilliseconds} ms.",
                    requestName,
                    featureName,
                    stopwatch.ElapsedMilliseconds);
            }
            else
            {
                _logger.LogInformation(
                    "Handled {RequestName} for feature {Feature} in {ElapsedMilliseconds} ms.",
                    requestName,
                    featureName,
                    stopwatch.ElapsedMilliseconds);
            }

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Failed handling {RequestName} for feature {Feature} after {ElapsedMilliseconds} ms. RequestData: {@RequestData}",
                requestName,
                featureName,
                stopwatch.ElapsedMilliseconds,
                requestData);

            throw;
        }
    }

    private static Dictionary<string, object?> GetSafeRequestData(TRequest request)
    {
        return request
            .GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => property.GetIndexParameters().Length == 0)
            .ToDictionary(
                property => property.Name,
                property => GetSafePropertyValue(property, request));
    }

    private static object? GetSafePropertyValue(PropertyInfo property, TRequest request)
    {
        if (SensitivePropertyNames.Contains(property.Name))
        {
            return "***";
        }

        var value = property.GetValue(request);

        if (value is IFormFile file)
        {
            return new
            {
                file.FileName,
                file.ContentType,
                file.Length
            };
        }

        if (value is string text && text.Length > 200)
        {
            return string.Concat(text.AsSpan(0, 200), "...");
        }

        return value;
    }
}
