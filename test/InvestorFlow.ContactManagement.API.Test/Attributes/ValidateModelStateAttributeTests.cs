using InvestorFlow.ContactManagement.API.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using ValidationException = InvestorFlow.ContactManagement.Domain.Exceptions.ValidationException;

namespace InvestorFlow.ContactManagement.API.Test.Attributes;

public class ValidateModelStateAttributeTests
{
    private readonly ValidateModelStateAttribute _sut = new();
    
    [Fact]
    public void Should_continue_without_exception_When_model_is_valid()
    {
        // Arrange
        var actionContext = CreateActionExecutingContext();

        // Act & Assert
        var exception = Record.Exception(() => _sut.OnActionExecuting(actionContext));

        // No exception is thrown when model state is valid
        Assert.Null(exception);
    }

    [Fact]
    public void Should_throw_validation_exception_When_request_When_model_is_invalid()
    {
        // Arrange
        var actionContext = CreateActionExecutingContext();
        actionContext.ModelState.AddModelError("Name", "Name is required");  // Invalid model state
        actionContext.ModelState.AddModelError("Age", "Age must be a positive integer");

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() => _sut.OnActionExecuting(actionContext));

        // Exception message contains the correct validation error messages
        Assert.Contains("Property: Name", exception.Message);
        Assert.Contains("Property: Age", exception.Message);
    }

    private static ActionExecutingContext CreateActionExecutingContext()
    {
        var httpContextMock = new Mock<HttpContext>();
        var actionContext = new ActionContext(
            httpContextMock.Object,
            new RouteData(),
            new ActionDescriptor(),
            new ModelStateDictionary());
        
        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            new object());
    }
}
