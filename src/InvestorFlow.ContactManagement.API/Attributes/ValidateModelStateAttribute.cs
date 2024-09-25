using InvestorFlow.ContactManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InvestorFlow.ContactManagement.API.Attributes
{
    /// <summary>
    /// Model state validation attribute
    /// </summary>
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before the action method is invoked
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid) return;

            var errorList = context.ModelState
                .Where(item => item.Value?.Errors.Count > 0)
                .Select(item => new
                {
                    Property = item.Key,
                    Errors = item.Value?.Errors
                        .Select(error => error.ErrorMessage)
                        .Distinct()
                })
                .Select(item => $"Property: {item.Property}, Error: {string.Join(" ", item.Errors ?? [])}")
                .ToList();

            throw new ValidationException($"{string.Join(" | ", errorList)}");
        }
    }
}
