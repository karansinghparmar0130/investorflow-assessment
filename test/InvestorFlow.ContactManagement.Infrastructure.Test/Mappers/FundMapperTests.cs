using AutoMapper;
using InvestorFlow.ContactManagement.Infrastructure.Mappers;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using DomainFund = InvestorFlow.ContactManagement.Domain.Entities.Fund;

namespace InvestorFlow.ContactManagement.Infrastructure.Test.Mappers;

public class FundMapperTests
{
    private const string Name = "test";
    private const string Email = "test@mail.com";
    private const string Phone = "0123456789";
    private readonly Guid _contactIdExternal = Guid.NewGuid();
    private const int ContactId = 1;
    private const string FundName = "Fund";
    private readonly Guid _fundIdExternal = Guid.NewGuid();
    private const int FundId = 2;
    private readonly FundMapper _sut;

    public FundMapperTests()
    {
        var mappingProfile = new FundMappingProfile();
        var configuration = new MapperConfiguration(config => config.AddProfile(mappingProfile));
        IMapper mapper = new Mapper(configuration);
        _sut = new FundMapper(mapper);
    }

    [Fact]
    private void Should_map_fund_entity_values_to_domain_fund()
    {
        // Arrange
        var fundEntity = new Fund
        {
            Name = FundName,
            Id = FundId,
            ExternalId = _fundIdExternal
        };

        var domainFund = new DomainFund
        {
            Name = FundName,
            Id = FundId,
            FundId = _fundIdExternal
        };

        // Act
        var actualResponse = _sut.Map(fundEntity);

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equivalent(domainFund, actualResponse, true);
    }
}
