using AutoMapper;
using InvestorFlow.ContactManagement.API.Mappers;
using InvestorFlow.ContactManagement.API.Models;
using InvestorFlow.ContactManagement.Domain.Entities;
using Contact = InvestorFlow.ContactManagement.API.Models.Contact;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.API.Test.Mappers;

public class ContactMapperTests
{
    private const string Name = "test";
    private const string Email = "test@mail.com";
    private const string Phone = "0123456789";
    private readonly Guid _contactId = Guid.NewGuid();
    private const string Name2 = "test2";
    private const string Email2 = "test2@mail.com";
    private const string Phone2 = "1234567890";
    private readonly Guid _contactId2 = Guid.NewGuid();
    private readonly Guid _fundId = Guid.NewGuid();
    private readonly ContactMapper _sut;

    public ContactMapperTests()
    {
        var mappingProfile = new ContactMappingProfile();
        var configuration = new MapperConfiguration(config => config.AddProfile(mappingProfile));
        IMapper mapper = new Mapper(configuration);
        _sut = new ContactMapper(mapper);
    }

    [Fact]
    private void Should_map_contact_values_to_domain_contact()
    {
        // Arrange
        var contact = new Contact
        {
            Name = Name,
            Email = Email,
            PhoneNumber = Phone
        };

        var domainContact = new DomainContact
        {
            Name = Name,
            Email = Email,
            PhoneNumber = Phone
        };

        // Act
        var actualResponse = _sut.Map(contact);

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equivalent(domainContact, actualResponse, true);
    }

    [Fact]
    private void Should_map_domain_contact_values_to_contact_response()
    {
        // Arrange
        var contactResponse = new ContactResponse
        {
            Name = Name,
            Email = Email,
            PhoneNumber = Phone,
            FundId = _fundId,
            ContactId = _contactId
        };

        var domainContact = new DomainContact
        {
            Name = Name,
            Email = Email,
            PhoneNumber = Phone,
            Fund = new Fund
            {
                FundId = _fundId,
                Name = "test fund"
            },
            ContactId = _contactId
        };

        // Act
        var actualResponse = _sut.Map(domainContact);

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equivalent(contactResponse, actualResponse, true);
    }

    [Fact]
    private void Should_map_domain_contact_values_to_fund_response()
    {
        // Arrange
        var contactResponseList = new List<ContactResponse>
        {
            new()
            {
                Name = Name,
                Email = Email,
                PhoneNumber = Phone,
                FundId = _fundId,
                ContactId = _contactId
            },
            new()
            {
                Name = Name2,
                Email = Email2,
                PhoneNumber = Phone2,
                FundId = _fundId,
                ContactId = _contactId2
            }
        };

        var domainContactList = new[]
        {
            new DomainContact
            {
                Name = Name,
                Email = Email,
                PhoneNumber = Phone,
                Fund = new Fund
                {
                    FundId = _fundId,
                    Name = "test fund"
                },
                ContactId = _contactId
            },
            new DomainContact
            {
                Name = Name2,
                Email = Email2,
                PhoneNumber = Phone2,
                Fund = new Fund
                {
                    FundId = _fundId,
                    Name = "test fund"
                },
                ContactId = _contactId2
            }
        };

        // Act
        var actualResponse = _sut.Map(domainContactList);

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equivalent(contactResponseList, actualResponse, true);
    }
}
