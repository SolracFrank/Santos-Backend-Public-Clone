using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EsApi.Middlewares;

public sealed class UnhandledExceptionMiddleware(
    RequestDelegate next,
    ILogger<UnhandledExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unhandled exception while processing request {TraceId}",
                context.TraceIdentifier);

            ReportToSentryIfNeeded(context, ex);

            if (context.Response.HasStarted)
            {
                logger.LogWarning(
                    "The response has already started. The exception handler will rethrow the exception. TraceId: {TraceId}",
                    context.TraceIdentifier);

                throw;
            }

            await WriteProblemDetailsAsync(context, ex);
        }
    }

    private static void ReportToSentryIfNeeded(HttpContext context, Exception ex)
    {
        if (ex is ApiException)
        {
            return;
        }

        SentrySdk.CaptureException(ex, scope =>
        {
            scope.SetTag("trace_id", context.TraceIdentifier);
            scope.SetTag("http.method", context.Request.Method);
            scope.SetTag("http.path", context.Request.Path.Value ?? string.Empty);

            scope.SetExtra("traceId", context.TraceIdentifier);
            scope.SetExtra("requestPath", context.Request.Path.Value);
            scope.SetExtra("queryString", context.Request.QueryString.Value);
        });
    }

    private static Task WriteProblemDetailsAsync(HttpContext context, Exception ex)
    {
        var (statusCode, title, detail, errorCode) = ex switch
        {
            ApiException apiException => (
                apiException.StatusCode,
                apiException.Title,
                apiException.Message,
                apiException.ErrorCode),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Unexpected server error.",
                "An unexpected error occurred while processing the request.",
                (string?)null)
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.TraceIdentifier
        };

        if (!string.IsNullOrWhiteSpace(errorCode))
        {
            problemDetails.Extensions["errorCode"] = errorCode;
        }

        var json = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(json);
    }
}