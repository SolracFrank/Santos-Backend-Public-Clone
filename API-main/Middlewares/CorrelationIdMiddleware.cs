using Microsoft.Extensions.Primitives;

namespace EsApi.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const string CorrelationIdHeaderName = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetCorrelationId(context.Request.Headers);
        context.TraceIdentifier = correlationId;
        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await next(context);
        }
    }

    private static string GetCorrelationId(IHeaderDictionary headers)
    {
        if (headers.TryGetValue(CorrelationIdHeaderName, out StringValues headerValue) &&
            !StringValues.IsNullOrEmpty(headerValue))
        {
            return headerValue.ToString();
        }

        return Guid.NewGuid().ToString("N");
    }
}
