using InvestorFlow.ContactManagement.API.Controllers;
using InvestorFlow.ContactManagement.API.Models;
using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;
using Microsoft.Extensions.Logging;

namespace InvestorFlow.ContactManagement.API.Test.Controllers;

public class FundControllerTests
{
    private readonly Mock<ILogger<FundController>> _loggerMock = new();
    private readonly Mock<IFundService> _fundServiceMock = new();
    private readonly Mock<IMapper<IEnumerable<DomainContact>, FundResponse>> _mapperMock = new();
    private readonly FundController _sut;

    public FundControllerTests() =>
        _sut = new FundController(
            _loggerMock.Object,
            _fundServiceMock.Object,
            _mapperMock.Object);

    [Fact]
    public async Task Should_return_response_When_get_contacts_from_fund_is_success()
    {
        // Arrange
        const string name1 = "Test";
        const string name2 = "Test";
        var fundId = Guid.NewGuid();
        var contactId1 = Guid.NewGuid();
        var contactId2 = Guid.NewGuid();
        var serviceResponse = new[]
        {
            new DomainContact { Name = name1, ContactId = contactId1 },
            new DomainContact { Name = name2, ContactId = contactId2 }
        };
        var mappedResponse = new FundResponse
        {
            new() { Name = name1, ContactId = contactId1 },
            new() { Name = name2, ContactId = contactId2 }
        };

        _fundServiceMock.Setup(x => x.ListContactsForFundAsync(It.IsAny<Guid>()))
            .ReturnsAsync(serviceResponse);
        _mapperMock.Setup(x => x.Map(It.IsAny<IEnumerable<DomainContact>>()))
            .Returns(mappedResponse);

        // Act
        var actualResponse = await _sut.GetContactsForFund(fundId) as OkObjectResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(200, actualResponse.StatusCode);
        Assert.Equal(mappedResponse, actualResponse.Value);
        _fundServiceMock.Verify(l => l.ListContactsForFundAsync(
                fundId),
            Times.Once);
        _mapperMock.Verify(l => l.Map(
                serviceResponse),
            Times.Once);
        _loggerMock.VerifyLogging(expectedMessage: "List contacts for fund completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_get_contacts_from_fund_is_failure()
    {
        var fundId = Guid.NewGuid();

        _fundServiceMock.Setup(x => x.ListContactsForFundAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.GetContactsForFund(fundId));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _fundServiceMock.Verify(l => l.ListContactsForFundAsync(
                fundId),
            Times.Once);
        _mapperMock.Verify(l => l.Map(
                It.IsAny<IEnumerable<DomainContact>>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }
}
