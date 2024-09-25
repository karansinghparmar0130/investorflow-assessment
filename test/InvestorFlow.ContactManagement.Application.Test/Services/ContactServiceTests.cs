using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Application.Services;
using InvestorFlow.ContactManagement.Domain.Entities;
using InvestorFlow.ContactManagement.Domain.Exceptions;

namespace InvestorFlow.ContactManagement.Application.Test.Services;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _contactRepositoryMock = new();
    private readonly Mock<IFundRepository> _fundRepositoryMock = new();
    private readonly ContactService _sut;

    public ContactServiceTests() =>
        _sut = new ContactService(_contactRepositoryMock.Object, _fundRepositoryMock.Object);

    [Fact]
    public async Task Should_return_contact_result_for_get_contact_When_contact_repository_returned_results()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var contact = new Contact { Name = "test" };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contact);

        // Act
        var actualResponse = await _sut.GetContactAsync(contactId);

        // Assert
        Assert.NotNull(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
    }

    [Fact]
    public async Task Should_throw_contact_not_found_exception_for_get_contact_When_contact_repository_returned_null()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        Contact? contact = null; // Returns null
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))!.ReturnsAsync(contact);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ContactNotFoundException>(async () => await _sut.GetContactAsync(contactId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ContactNotFoundException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
    }

    [Fact]
    public async Task Should_return_contact_result_for_create_contact_When_contact_repository_returned_results()
    {
        // Arrange
        var contact = new Contact { Name = "test" };
        _contactRepositoryMock.Setup(x => x.AddAsync(It.IsAny<Contact>())).ReturnsAsync(contact);

        // Act
        var actualResponse = await _sut.CreateContactAsync(contact);

        // Assert
        Assert.NotNull(actualResponse);
        _contactRepositoryMock.Verify(l => l.AddAsync(
                It.Is<Contact>(c => c.ContactId != null)),
            Times.Once);
    }

    [Fact]
    public async Task Should_return_contact_result_for_update_contact_When_contact_repository_returned_results()
    {
        // Arrange
        const string name = "test";
        const string email = "email@test";
        const string phone = "1234";
        var contactId = Guid.NewGuid();
        var contactById = new Contact { Name = name };
        var contact = new Contact { Name = name, Email = email, PhoneNumber = phone };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _contactRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Contact>())).ReturnsAsync(contact);

        // Act
        var actualResponse = await _sut.UpdateContactAsync(contact, contactId);

        // Assert
        Assert.NotNull(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.Is<Contact>(c => c.Email == email && c.Name == name && c.PhoneNumber == phone)),
            Times.Once);
    }

    [Fact]
    public async Task
        Should_throw_contact_not_found_exception_for_update_contact_When_contact_repository_returned_null()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        Contact? contact = null; // Returns null
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))!.ReturnsAsync(contact);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ContactNotFoundException>(async () =>
                await _sut.UpdateContactAsync(contact!, contactId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ContactNotFoundException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.IsAny<Contact>()),
            Times.Never);
    }

    [Fact]
    public async Task
        Should_return_contact_result_for_assign_contact_to_fund_When_contact_and_fund_repository_returned_results()
    {
        // Arrange
        const string name = "test";
        const string email = "email@test";
        const string phone = "1234";
        var fundId = Guid.NewGuid();
        var fund = new Fund { Name = "fund", FundId = fundId };
        var contactId = Guid.NewGuid();
        var contactById = new Contact { Name = name };
        var contact = new Contact { Name = name, Email = email, PhoneNumber = phone, Fund = fund };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _fundRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(fund);
        _contactRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Contact>())).ReturnsAsync(contact);

        // Act
        var actualResponse = await _sut.AssignContactToFundAsync(contactId, fundId);

        // Assert
        Assert.NotNull(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _fundRepositoryMock.Verify(l => l.GetAsync(
                fundId, false),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.Is<Contact>(c => c.Fund == fund)),
            Times.Once);
    }

    [Fact]
    public async Task
        Should_throw_contact_not_found_exception_for_assign_contact_to_fund_When_contact_repository_returned_null()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var fundId = Guid.NewGuid();
        var contactById = new Contact { Name = "test" };
        Fund? fund = null; // Returns null
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _fundRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>()))!.ReturnsAsync(fund);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<FundNotFoundException>(async () =>
                await _sut.AssignContactToFundAsync(contactId, fundId));

        Assert.NotNull(actualResponse);
        Assert.IsType<FundNotFoundException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _fundRepositoryMock.Verify(l => l.GetAsync(
                fundId, false),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.IsAny<Contact>()),
            Times.Never);
    }

    [Fact]
    public async Task
        Should_throw_fund_not_found_exception_for_assign_contact_to_fund_When_fund_repository_returned_null()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        var fundId = Guid.NewGuid();
        Contact? contact = null; // Returns null
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))!.ReturnsAsync(contact);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ContactNotFoundException>(async () =>
                await _sut.AssignContactToFundAsync(contactId, fundId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ContactNotFoundException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _fundRepositoryMock.Verify(l => l.GetAsync(
                It.IsAny<Guid>(), It.IsAny<bool>()),
            Times.Never);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.IsAny<Contact>()),
            Times.Never);
    }

    [Fact]
    public async Task
        Should_throw_validation_exception_for_assign_contact_to_fund_When_same_contact_is_assigned_to_fund()
    {
        // Arrange
        const string name = "test";
        var contactId = Guid.NewGuid();
        var fundId = Guid.NewGuid();
        var contactById = new Contact
            { Name = name, ContactId = contactId, Fund = new Fund { FundId = fundId, Name = "some name" } };
        var fund = new Fund { Name = "fund", FundId = fundId };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _fundRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>(), It.IsAny<bool>())).ReturnsAsync(fund);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ValidationException>(async () =>
                await _sut.AssignContactToFundAsync(contactId, fundId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _fundRepositoryMock.Verify(l => l.GetAsync(
                fundId, false),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.IsAny<Contact>()),
            Times.Never);
    }

    [Fact]
    public async Task
        Should_return_contact_result_for_remove_contact_for_fund_When_contact_repository_returned_results()
    {
        // Arrange
        const string name = "test";
        var contactId = Guid.NewGuid();
        var contactById = new Contact { Name = name, Fund = new Fund { Name = "Global", FundId = Guid.NewGuid() } };
        var contact = new Contact { Name = name };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _contactRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Contact>())).ReturnsAsync(contact);

        // Act
        var actualResponse = await _sut.RemoveContactFromFundAsync(contactId);

        // Assert
        Assert.NotNull(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.Is<Contact>(c => c.Fund == null)),
            Times.Once);
    }

    [Fact]
    public async Task
        Should_throw_contact_not_found_exception_for_remove_contact_for_fund_When_contact_repository_returned_null()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        Contact? contact = null; // Returns null
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))!.ReturnsAsync(contact);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ContactNotFoundException>(async () =>
                await _sut.RemoveContactFromFundAsync(contactId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ContactNotFoundException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.UpdateAsync(
                It.IsAny<Contact>()),
            Times.Never);
    }

    [Fact]
    public async Task
        Should_return_contact_result_for_delete_contact_When_contact_repository_returned_results()
    {
        // Arrange
        const string name = "test";
        var contactId = Guid.NewGuid();
        var contactById = new Contact { Name = name };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _contactRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));

        // Act
        await _sut.DeleteContactAsync(contactId);

        // Assert
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.DeleteAsync(
                contactId),
            Times.Once);
    }

    [Fact]
    public async Task
        Should_throw_validation_exception_for_delete_contact_When_contact_has_fund_assigned()
    {
        // Arrange
        const string name = "test";
        var contactId = Guid.NewGuid();
        var contactById = new Contact { Name = name, Fund = new Fund { Name = "Global", FundId = Guid.NewGuid() } };
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>())).ReturnsAsync(contactById);
        _contactRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<Guid>()));

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ValidationException>(async () =>
                await _sut.DeleteContactAsync(contactId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ValidationException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.DeleteAsync(
                It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task
        Should_throw_contact_not_found_exception_for_delete_contact_When_contact_repository_returned_null()
    {
        // Arrange
        var contactId = Guid.NewGuid();
        Contact? contact = null; // Returns null
        _contactRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Guid>()))!.ReturnsAsync(contact);

        // Act & Assert
        var actualResponse =
            await Assert.ThrowsAsync<ContactNotFoundException>(async () =>
                await _sut.DeleteContactAsync(contactId));

        Assert.NotNull(actualResponse);
        Assert.IsType<ContactNotFoundException>(actualResponse);
        _contactRepositoryMock.Verify(l => l.GetAsync(
                contactId),
            Times.Once);
        _contactRepositoryMock.Verify(l => l.DeleteAsync(
                It.IsAny<Guid>()),
            Times.Never);
    }
}
