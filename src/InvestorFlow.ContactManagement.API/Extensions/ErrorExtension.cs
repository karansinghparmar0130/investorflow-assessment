using InvestorFlow.ContactManagement.API.Models;

namespace InvestorFlow.ContactManagement.API.Extensions;

public static class ErrorExtension
{
    /// <summary>
    /// Create a standard error response for the application
    /// </summary>
    /// <param name="code">Error code</param>
    /// <param name="message">Error message</param>
    /// <returns>Error response object</returns>
    public static ErrorResponse CreateErrorResponse(string code, string message) =>
        new()
        {
            Code = code,
            Message = message
        };
}
