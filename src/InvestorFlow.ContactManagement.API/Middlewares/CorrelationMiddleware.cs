namespace InvestorFlow.ContactManagement.API.Middlewares;

/// <summary>
/// Middleware to add/generate Correlation Id for grouping logs
/// </summary>
/// <param name="next">next in context</param>
/// <param name="logger">logger instance</param>
public class CorrelationMiddleware(
    RequestDelegate next,
    ILogger<CorrelationMiddleware> logger)
{
    // This header can be provided, to correlate to upstream sources
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task Invoke(HttpContext context)
    {
        context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationIds);
        var correlationId = correlationIds.FirstOrDefault() ?? Guid.NewGuid().ToString();

        using (logger.BeginScope("{CorrelationId}", correlationId))
            await next(context);
    }
}
