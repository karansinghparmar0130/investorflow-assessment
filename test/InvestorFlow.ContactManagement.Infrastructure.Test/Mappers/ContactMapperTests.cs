using AutoMapper;
using InvestorFlow.ContactManagement.Infrastructure.Mappers;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.Infrastructure.Test.Mappers;

public class ContactMapperTests
{
    private const string Name = "test";
    private const string Email = "test@mail.com";
    private const string Phone = "0123456789";
    private readonly Guid _contactIdExternal = Guid.NewGuid();
    private const int ContactId = 1;
    private const string FundName = "Fund";
    private readonly Guid _fundIdExternal = Guid.NewGuid();
    private const int FundId = 2;
    private readonly ContactMapper _sut;
    
    public ContactMapperTests()
    {
        var mappingProfile = new ContactMappingProfile();
        var configuration = new MapperConfiguration(config => config.AddProfile(mappingProfile));
        IMapper mapper = new Mapper(configuration);
        _sut = new ContactMapper(mapper);
    }

    [Fact]
    private void Should_map_domain_contact_values_to_contact_entity()
    {
        // Arrange
        var contactEntity = new Contact
        {
            Id = ContactId,
            Name = Name,
            Email = Email,
            PhoneNumber = Phone,
            FundId = FundId,
            ExternalId = _contactIdExternal,
            Fund = new Fund
            {
                Id = FundId,
                ExternalId = _fundIdExternal,
                Name = FundName
            }
        };

        var domainContact = new DomainContact
        {
            Name = Name,
            Email = Email,
            PhoneNumber = Phone,
            Fund = new Domain.Entities.Fund
            {
                Id = FundId,
                FundId = _fundIdExternal,
                Name = FundName
            },
            ContactId = _contactIdExternal,
            Id = ContactId
        };

        // Act
        var actualResponse = _sut.Map(domainContact);

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equivalent(contactEntity, actualResponse, true);
    }
    
    [Fact]
    private void Should_map_contact_entity_values_to_domain_contact()
    {
        // Arrange
        var contactEntity = new Contact
        {
            Id = ContactId,
            Name = Name,
            Email = Email,
            PhoneNumber = Phone,
            FundId = FundId,
            ExternalId = _contactIdExternal,
            Fund = new Fund
            {
                Id = FundId,
                ExternalId = _fundIdExternal,
                Name = FundName
            }
        };

        var domainContact = new DomainContact
        {
            Name = Name,
            Email = Email,
            PhoneNumber = Phone,
            Fund = new Domain.Entities.Fund
            {
                Id = FundId,
                FundId = _fundIdExternal,
                Name = FundName
            },
            ContactId = _contactIdExternal,
            Id = ContactId
        };

        // Act
        var actualResponse = _sut.Map(contactEntity);

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equivalent(domainContact, actualResponse, true);
    }
}
