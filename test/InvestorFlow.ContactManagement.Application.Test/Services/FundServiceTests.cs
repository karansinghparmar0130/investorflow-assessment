using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Application.Services;
using InvestorFlow.ContactManagement.Domain.Entities;
using InvestorFlow.ContactManagement.Domain.Exceptions;

namespace InvestorFlow.ContactManagement.Application.Test.Services;

public class FundServiceTests
{
    private readonly Mock<IFundRepository> _fundRepositoryMock = new();
    private readonly FundService _sut;

    public FundServiceTests() =>
        _sut = new FundService(_fundRepositoryMock.Object);

    [Fact]
    public async Task Should_return_contact_list__for_get_contact_for_fund_When_fund_repository_returned_results()
    {
        // Arrange
        var fundId = Guid.NewGuid();
        var fund = new Fund { Name = "fund", FundId = fundId };
        _fundRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(fund);

        // Act
        var actualResponse = await _sut.ListContactsForFundAsync(fundId);

        // Assert
        Assert.NotNull(actualResponse);
        _fundRepositoryMock.Verify(l => l.GetAsync(
                fundId, true),
            Times.Once);
    }

    [Fact]
    public async Task Should_throw_fund_not_found_exception_for_get_contact_for_fund_When_fund_repository_returned_null()
    {
        // Arrange
        var fundId = Guid.NewGuid();
        Fund? fund = null; // Returns null
        _fundRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))!.ReturnsAsync(fund);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<FundNotFoundException>(async () => await _sut.ListContactsForFundAsync(fundId));
        
        Assert.NotNull(actualResponse);
        Assert.IsType<FundNotFoundException>(actualResponse);
        _fundRepositoryMock.Verify(l => l.GetAsync(
                fundId, true),
            Times.Once);
    }
}
