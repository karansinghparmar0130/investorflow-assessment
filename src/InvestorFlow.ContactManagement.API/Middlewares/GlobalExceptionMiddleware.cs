using InvestorFlow.ContactManagement.API.Extensions;
using InvestorFlow.ContactManagement.Domain.Exceptions;

namespace InvestorFlow.ContactManagement.API.Middlewares;

/// <summary>
/// Middleware to handle exceptions globally and return abstract responses to consumer
/// </summary>
/// <param name="next">next in context</param>
/// <param name="logger">logger instance</param>
public class GlobalExceptionMiddleware(
    RequestDelegate next, 
    ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch(Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        logger.LogError(exception, "Exception occured in Contact Management API");

        var (statusCode, message) = exception switch
        {
            ValidationException =>
                (StatusCodes.Status400BadRequest, "Refer API documentation for request specification"),
            ContactNotFoundException =>
                (StatusCodes.Status404NotFound, "Contact not found"),
            FundNotFoundException =>
                (StatusCodes.Status404NotFound, "Fund not found"),
            InfrastructureException =>
                (StatusCodes.Status500InternalServerError, "Refer API logs for further details"),
            _ =>
                (StatusCodes.Status500InternalServerError, "Refer API logs for further details")
        };
        
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(
            ErrorExtension.CreateErrorResponse(statusCode.ToString(), message));
    }
}
