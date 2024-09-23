using InvestorFlow.ContactManagement.API.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InvestorFlow.ContactManagement.API.Test.Middlewares;

public class CorrelationMiddlewareTests
{
    private readonly Mock<ILogger<CorrelationMiddleware>> _loggerMock = new();
    private readonly Mock<RequestDelegate> _requestDelegateMock = new();
    private readonly CorrelationMiddleware _sut;

    public CorrelationMiddlewareTests() =>
        _sut = new CorrelationMiddleware(_requestDelegateMock.Object, _loggerMock.Object);

    [Fact]
    public async Task Should_generate_correlationId_When_request_does_not_have_correlation_header()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var request = new DefaultHttpContext();
        request.Request.Headers["Some-header"] = correlationId;
        
        // Act
        await _sut.Invoke(request);

        // Assert
        _loggerMock.Verify(
            logger => logger.BeginScope(It.Is<It.IsAnyType>((o, t) => !o.ToString()!.Contains(correlationId))), 
            Times.Once);
    }
    
    [Fact]
    public async Task Should_use_correlationId_from_header_When_request_has_correlation_header()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        var request = new DefaultHttpContext();
        request.Request.Headers["X-Correlation-ID"] = correlationId;
        
        // Act
        await _sut.Invoke(request);
        
        // Assert
        _loggerMock.Verify(
            logger => logger.BeginScope(It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains(correlationId))), 
            Times.Once);
    }
}
