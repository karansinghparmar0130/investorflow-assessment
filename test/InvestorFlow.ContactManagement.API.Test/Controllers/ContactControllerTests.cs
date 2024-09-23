using InvestorFlow.ContactManagement.API.Controllers;
using InvestorFlow.ContactManagement.API.Models;
using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.API.Test.Controllers;

public class ContactControllerTests
{
    private readonly Mock<ILogger<ContactController>> _loggerMock = new();
    private readonly Mock<IContactService> _contactServiceMock = new();
    private readonly Mock<IMapper<Contact, DomainContact>> _contactMapperMock = new();
    private readonly Mock<IMapper<DomainContact, ContactResponse>> _contactResponseMapperMock = new();
    private readonly ContactController _sut;

    public ContactControllerTests() =>
        _sut = new ContactController(
            _loggerMock.Object,
            _contactServiceMock.Object,
            _contactMapperMock.Object,
            _contactResponseMapperMock.Object);

    [Fact]
    public async Task Should_return_response_When_create_contact_is_success()
    {
        // Arrange
        const string name = "Test";
        var contactId = Guid.NewGuid();
        var request = new Contact { Name = name };
        var mappedRequest = new DomainContact { Name = name };
        var serviceResponse = new DomainContact { Name = name, ContactId = contactId };
        var mappedResponse = new ContactResponse { Name = name, ContactId = contactId };

        _contactMapperMock.Setup(x => x.Map(It.IsAny<Contact>()))
            .Returns(mappedRequest);
        _contactServiceMock.Setup(x => x.CreateContactAsync(It.IsAny<DomainContact>()))
            .ReturnsAsync(serviceResponse);
        _contactResponseMapperMock.Setup(x => x.Map(It.IsAny<DomainContact>()))
            .Returns(mappedResponse);

        // Act
        var actualResponse = await _sut.CreateContact(request) as OkObjectResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(200, actualResponse.StatusCode);
        Assert.Equal(mappedResponse, actualResponse.Value);
        _contactMapperMock.Verify(l => l.Map(
                request),
            Times.Once);
        _contactServiceMock.Verify(l => l.CreateContactAsync(
                mappedRequest),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                serviceResponse),
            Times.Once);
        _loggerMock.VerifyLogging(expectedMessage: "Create contact completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_create_contact_is_failure()
    {
        const string name = "Test";
        var request = new Contact { Name = name };
        var mappedRequest = new DomainContact { Name = name };

        _contactMapperMock.Setup(x => x.Map(It.IsAny<Contact>()))
            .Returns(mappedRequest);
        _contactServiceMock.Setup(x => x.CreateContactAsync(It.IsAny<DomainContact>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.CreateContact(request));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactMapperMock.Verify(l => l.Map(
                request),
            Times.Once);
        _contactServiceMock.Verify(l => l.CreateContactAsync(
                mappedRequest),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }

    [Fact]
    public async Task Should_return_response_When_update_contact_is_success()
    {
        // Arrange
        const string name = "Test";
        var contactId = Guid.NewGuid();
        var request = new Contact { Name = name };
        var mappedRequest = new DomainContact { Name = name };
        var serviceResponse = new DomainContact { Name = name, ContactId = contactId };
        var mappedResponse = new ContactResponse { Name = name, ContactId = contactId };

        _contactMapperMock.Setup(x => x.Map(It.IsAny<Contact>()))
            .Returns(mappedRequest);
        _contactServiceMock.Setup(x => x.UpdateContactAsync(It.IsAny<DomainContact>(), It.IsAny<Guid>()))
            .ReturnsAsync(serviceResponse);
        _contactResponseMapperMock.Setup(x => x.Map(It.IsAny<DomainContact>()))
            .Returns(mappedResponse);

        // Act
        var actualResponse = await _sut.UpdateContact(request, contactId) as OkObjectResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(200, actualResponse.StatusCode);
        Assert.Equal(mappedResponse, actualResponse.Value);
        _contactMapperMock.Verify(l => l.Map(
                request),
            Times.Once);
        _contactServiceMock.Verify(l => l.UpdateContactAsync(
                mappedRequest, contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                serviceResponse),
            Times.Once);
        _loggerMock.VerifyLogging(expectedMessage: "Update contact completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_update_contact_is_failure()
    {
        const string name = "Test";
        var contactId = Guid.NewGuid();
        var request = new Contact { Name = name };
        var mappedRequest = new DomainContact { Name = name };

        _contactMapperMock.Setup(x => x.Map(It.IsAny<Contact>()))
            .Returns(mappedRequest);
        _contactServiceMock.Setup(x => x.UpdateContactAsync(It.IsAny<DomainContact>(), It.IsAny<Guid>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.UpdateContact(request, contactId));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactMapperMock.Verify(l => l.Map(
                request),
            Times.Once);
        _contactServiceMock.Verify(l => l.UpdateContactAsync(
                mappedRequest, contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }

    [Fact]
    public async Task Should_return_response_When_get_contact_is_success()
    {
        // Arrange
        const string name = "Test";
        var contactId = Guid.NewGuid();
        var serviceResponse = new DomainContact { Name = name, ContactId = contactId };
        var mappedResponse = new ContactResponse { Name = name, ContactId = contactId };

        _contactServiceMock.Setup(x => x.GetContactAsync(It.IsAny<Guid>()))
            .ReturnsAsync(serviceResponse);
        _contactResponseMapperMock.Setup(x => x.Map(It.IsAny<DomainContact>()))
            .Returns(mappedResponse);

        // Act
        var actualResponse = await _sut.GetContact(contactId) as OkObjectResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(200, actualResponse.StatusCode);
        Assert.Equal(mappedResponse, actualResponse.Value);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.GetContactAsync(
                contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                serviceResponse),
            Times.Once);
        _loggerMock.VerifyLogging(expectedMessage: "Get contact completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_get_contact_is_failure()
    {
        var contactId = Guid.NewGuid();

        _contactServiceMock.Setup(x => x.GetContactAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.GetContact(contactId));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.GetContactAsync(
                contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }

    [Fact]
    public async Task Should_return_response_When_delete_contact_is_success()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        _contactServiceMock.Setup(x => x.GetContactAsync(It.IsAny<Guid>()));

        // Act
        var actualResponse = await _sut.DeleteContact(contactId) as NoContentResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(204, actualResponse.StatusCode);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.DeleteContactAsync(
                contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedMessage: "Delete contact completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_delete_contact_is_failure()
    {
        var contactId = Guid.NewGuid();

        _contactServiceMock.Setup(x => x.DeleteContactAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.DeleteContact(contactId));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.DeleteContactAsync(
                contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }

    [Fact]
    public async Task Should_return_response_When_assign_contact_to_fund_is_success()
    {
        // Arrange
        const string name = "Test";
        var contactId = Guid.NewGuid();
        var fundId = Guid.NewGuid();
        var serviceResponse = new DomainContact { Name = name, ContactId = contactId };
        var mappedResponse = new ContactResponse { Name = name, ContactId = contactId };

        _contactServiceMock.Setup(x => x.AssignContactToFundAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(serviceResponse);
        _contactResponseMapperMock.Setup(x => x.Map(It.IsAny<DomainContact>()))
            .Returns(mappedResponse);

        // Act
        var actualResponse = await _sut.AssignFundToContact(contactId, fundId) as OkObjectResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(200, actualResponse.StatusCode);
        Assert.Equal(mappedResponse, actualResponse.Value);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.AssignContactToFundAsync(
                contactId, fundId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                serviceResponse),
            Times.Once);
        _loggerMock.VerifyLogging(expectedMessage: "Assign contact to fund completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_assign_contact_to_fund_is_failure()
    {
        var contactId = Guid.NewGuid();
        var fundId = Guid.NewGuid();

        _contactServiceMock.Setup(x => x.AssignContactToFundAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.AssignFundToContact(contactId, fundId));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.AssignContactToFundAsync(
                contactId, fundId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }

    [Fact]
    public async Task Should_return_response_When_remove_contact_from_fund_is_success()
    {
        // Arrange
        const string name = "Test";
        var contactId = Guid.NewGuid();
        var serviceResponse = new DomainContact { Name = name, ContactId = contactId };
        var mappedResponse = new ContactResponse { Name = name, ContactId = contactId };

        _contactServiceMock.Setup(x => x.RemoveContactFromFundAsync(It.IsAny<Guid>()))
            .ReturnsAsync(serviceResponse);
        _contactResponseMapperMock.Setup(x => x.Map(It.IsAny<DomainContact>()))
            .Returns(mappedResponse);

        // Act
        var actualResponse = await _sut.RemoveFundFromContact(contactId) as OkObjectResult;

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(200, actualResponse.StatusCode);
        Assert.Equal(mappedResponse, actualResponse.Value);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.RemoveContactFromFundAsync(
                contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                serviceResponse),
            Times.Once);
        _loggerMock.VerifyLogging(expectedMessage: "Remove fund from contact completed",
            expectedLogLevel: LogLevel.Information, times: Times.Once());
    }

    [Fact]
    public async Task Should_throw_error_When_remove_contact_from_fund_is_failure()
    {
        var contactId = Guid.NewGuid();

        _contactServiceMock.Setup(x => x.RemoveContactFromFundAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new ValidationException("failed"));

        var actualResponse = await Assert.ThrowsAsync<ValidationException>(async () =>
            await _sut.RemoveFundFromContact(contactId));

        // Assert
        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactMapperMock.Verify(l => l.Map(
                It.IsAny<Contact>()),
            Times.Never);
        _contactServiceMock.Verify(l => l.RemoveContactFromFundAsync(
                contactId),
            Times.Once);
        _contactResponseMapperMock.Verify(l => l.Map(
                It.IsAny<DomainContact>()),
            Times.Never);
        _loggerMock.VerifyLogging(expectedLogLevel: LogLevel.Information, times: Times.Never());
    }
}
