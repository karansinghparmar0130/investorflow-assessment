using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using InvestorFlow.ContactManagement.API.Middlewares;
using InvestorFlow.ContactManagement.Domain.Exceptions;

namespace InvestorFlow.ContactManagement.API.Test.Middlewares;

public class GlobalExceptionMiddlewareTests
{
    private readonly Mock<ILogger<GlobalExceptionMiddleware>> _loggerMock = new();

    [Fact]
    public async Task Should_not_do_anything_When_no_exception()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate mockNext = ctx => Task.CompletedTask;
        var sut = new GlobalExceptionMiddleware(mockNext, _loggerMock.Object);

        // Act
        await sut.Invoke(context);

        // Assert
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Error, times: Times.Never());
    }

    [Fact]
    public async Task Should_return_bad_request_When_validation_exception()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate mockNext = ctx => throw new ValidationException();
        var sut = new GlobalExceptionMiddleware(mockNext, _loggerMock.Object);

        // Act
        await sut.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        _loggerMock.VerifyLogging(expectedMessage: "Exception occured in Contact Management API",
            expectedLogLevel: LogLevel.Error, times: Times.Once());
    }

    [Fact]
    public async Task Should_return_not_found_When_fund_not_found_exception()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate mockNext = ctx => throw new FundNotFoundException();
        var sut = new GlobalExceptionMiddleware(mockNext, _loggerMock.Object);

        // Act
        await sut.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        _loggerMock.VerifyLogging(expectedMessage: "Exception occured in Contact Management API",
            expectedLogLevel: LogLevel.Error, times: Times.Once());
    }

    [Fact]
    public async Task Should_return_not_found_When_cart_not_found_exception()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate mockNext = ctx => throw new ContactNotFoundException();
        var sut = new GlobalExceptionMiddleware(mockNext, _loggerMock.Object);

        // Act
        await sut.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        _loggerMock.VerifyLogging(expectedMessage: "Exception occured in Contact Management API",
            expectedLogLevel: LogLevel.Error, times: Times.Once());
    }

    [Fact]
    public async Task Should_return_internal_server_error_When_infrastructure_exception()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate mockNext = ctx => throw new InfrastructureException();
        var sut = new GlobalExceptionMiddleware(mockNext, _loggerMock.Object);

        // Act
        await sut.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        _loggerMock.VerifyLogging(expectedMessage: "Exception occured in Contact Management API",
            expectedLogLevel: LogLevel.Error, times: Times.Once());
    }

    [Fact]
    public async Task Should_return_internal_server_error_When_generic_exception()
    {
        // Arrange
        var context = new DefaultHttpContext();
        RequestDelegate mockNext = ctx => throw new Exception("Generic error");
        var sut = new GlobalExceptionMiddleware(mockNext, _loggerMock.Object);


        // Act
        await sut.Invoke(context);

        // Assert
        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        _loggerMock.VerifyLogging(expectedMessage: "Exception occured in Contact Management API",
            expectedLogLevel: LogLevel.Error, times: Times.Once());
    }
}
