using System.Net;
using System.Net.Http.Json;
using System.Text;
using InvestorFlow.ContactManagement.API.Models;
using InvestorFlow.ContactManagement.Integration.Test.Attributes;
using InvestorFlow.ContactManagement.Integration.Test.Helpers;

namespace InvestorFlow.ContactManagement.Integration.Test;

[TestCaseOrderer(
    ordererTypeName: "InvestorFlow.ContactManagement.Integration.Test.Helpers.PriorityOrderer",
    ordererAssemblyName: "InvestorFlow.ContactManagement.Integration.Test")]
public class ApiTests(TestWebAppFactory factory, TestFixture testFixture) :
    TestWebAppBase(factory),
    IClassFixture<TestFixture>
{
    private const string Name = "test";
    private const string Email = "test@mail.com";
    private const string Phone = "0123456789";

    [Fact, TestPriority(1)]
    public async Task Step_1_Should_create_contact_When_create_is_called()
    {
        // Arrange
        const string url = "/api/v1/contact";
        using var request = new StringContent(
            new Contact
            {
                Name = Name,
                Email = Email,
                PhoneNumber = Phone
            }.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Act
        using var httpResponse = await Client.PostAsync(url, request);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.Created, httpResponse.StatusCode);
        // Original values should be returned
        Assert.Equal(Name, actualResponse.Name);
        Assert.Equal(Email, actualResponse.Email);
        Assert.Equal(Phone, actualResponse.PhoneNumber);
        // ContactId should be created and returned
        Assert.NotNull(actualResponse.ContactId);
        // There shouldn't be any fund assigned
        Assert.Null(actualResponse.FundId);
        // Assign values for subsequent tests
        testFixture.ContactId = actualResponse.ContactId;
    }

    [Fact, TestPriority(2)]
    public async Task Step_2_Should_get_contact_When_contact_is_created()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(Name, actualResponse.Name);
        Assert.Equal(Email, actualResponse.Email);
        Assert.Equal(Phone, actualResponse.PhoneNumber);
        // Original ContactId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        // There shouldn't be any fund assigned
        Assert.Null(actualResponse.FundId);
    }

    [Fact, TestPriority(3)]
    public async Task Step_3_Should_update_contact_When_update_is_called()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";
        using var request = new StringContent(
            new Contact
            {
                Name = Name,
                Email = null,
                PhoneNumber = null
            }.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Act
        using var httpResponse = await Client.PutAsync(url, request);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        // Original values should be returned
        Assert.Equal(Name, actualResponse.Name);
        // Should be set to null
        Assert.Null(actualResponse.Email);
        Assert.Null(actualResponse.PhoneNumber);
        // Original ContactId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        // There shouldn't be any fund assigned
        Assert.Null(actualResponse.FundId);
    }

    [Fact, TestPriority(4)]
    public async Task Step_4_Should_get_contact_When_contact_is_updated()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(Name, actualResponse.Name);
        // Should be set to null
        Assert.Null(actualResponse.Email);
        Assert.Null(actualResponse.PhoneNumber);
        // Original ContactId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        // There shouldn't be any fund assigned
        Assert.Null(actualResponse.FundId);
    }
    
    [Fact, TestPriority(5)]
    public async Task Step_5_Should_return_empty_list_When_fund_does_not_have_any_contact_assigned()
    {
        // Arrange
        var fundId = testFixture.FundId.ToString();
        var url = $"/api/v1/fund/{fundId}/contacts";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<FundResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        // Returns empty list of contacts
        Assert.Empty(actualResponse);
    }
    
    [Fact, TestPriority(6)]
    public async Task Step_6_Should_assign_contact_to_fund_When_assign_to_fund_is_called()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var fundId = testFixture.FundId.ToString();
        var url = $"/api/v1/contact/{contactId}/assign-fund/{fundId}";

        // Act
        using var httpResponse = await Client.PutAsync(url, null);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        // Original values should be returned
        Assert.Equal(Name, actualResponse.Name);
        // Should be set to null
        Assert.Null(actualResponse.Email);
        Assert.Null(actualResponse.PhoneNumber);
        // Original ContactId/ FundId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        Assert.Equal(testFixture.FundId, actualResponse.FundId);
    }
    
    [Fact, TestPriority(7)]
    public async Task Step_7_Should_return_400_When_assign_to_fund_is_called_for_same_contact()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var fundId = testFixture.FundId.ToString();
        var url = $"/api/v1/contact/{contactId}/assign-fund/{fundId}";

        // Act
        using var httpResponse = await Client.PutAsync(url, null);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        Assert.Equal("400", actualResponse.Code);
        Assert.Contains("Contact is already assigned to this fund", actualResponse.Message);
    }
    
    [Fact, TestPriority(8)]
    public async Task Step_8_Should_return_contact_list_When_fund_has_contact_assigned()
    {
        // Arrange
        var fundId = testFixture.FundId.ToString();
        var url = $"/api/v1/fund/{fundId}/contacts";
        var contactList = new FundResponse
        {
            new()
            {
                ContactId = testFixture.ContactId,
                FundId = testFixture.FundId,
                Name = Name,
                PhoneNumber = null,
                Email = null
            }
        };

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<FundResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        // Returns empty list of contacts
        Assert.Equivalent(contactList, actualResponse, strict: true);
    }
    
    [Fact, TestPriority(9)]
    public async Task Step_9_Should_get_contact_When_contact_is_assigned_to_fund()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(Name, actualResponse.Name);
        // Should be set to null
        Assert.Null(actualResponse.Email);
        Assert.Null(actualResponse.PhoneNumber);
        // Original ContactId/ FundId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        Assert.Equal(testFixture.FundId, actualResponse.FundId);
    }

    [Fact, TestPriority(10)]
    public async Task Step_10_Should_return_400_When_contact_is_assigned_on_fund_and_delete_is_called()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.DeleteAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        Assert.Equal("400", actualResponse.Code);
        Assert.Equal("Contact is assigned to a fund", actualResponse.Message);
    }
    
    [Fact, TestPriority(11)]
    public async Task Step_11_Should_remove_contact_from_fund_When_remove_from_fund_is_called()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}/remove-fund";

        // Act
        using var httpResponse = await Client.PutAsync(url, null);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        // Original values should be returned
        Assert.Equal(Name, actualResponse.Name);
        // Should be set to null
        Assert.Null(actualResponse.Email);
        Assert.Null(actualResponse.PhoneNumber);
        // Original ContactId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        // There shouldn't be any fund assigned
        Assert.Null(actualResponse.FundId);
    }
    
    [Fact, TestPriority(12)]
    public async Task Step_12_Should_get_contact_When_contact_has_been_removed_from_fund()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ContactResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        Assert.Equal(Name, actualResponse.Name);
        // Should be set to null
        Assert.Null(actualResponse.Email);
        Assert.Null(actualResponse.PhoneNumber);
        // Original ContactId should be returned
        Assert.Equal(testFixture.ContactId, actualResponse.ContactId);
        // There shouldn't be any fund assigned
        Assert.Null(actualResponse.FundId);
    }
    
    [Fact, TestPriority(13)]
    public async Task Step_13_Should_return_empty_list_When_contact_has_been_removed_from_fund()
    {
        // Arrange
        var fundId = testFixture.FundId.ToString();
        var url = $"/api/v1/fund/{fundId}/contacts";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<FundResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
        // Returns empty list of contacts
        Assert.Empty(actualResponse);
    }
    
    [Fact, TestPriority(14)]
    public async Task Step_14_Should_return_204_When_delete_is_called()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.DeleteAsync(url);
        var actualResponse = await httpResponse.Content.ReadAsStringAsync();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.NoContent, httpResponse.StatusCode);
    }
    
    [Fact, TestPriority(15)]
    public async Task Step_15_Should_return_404_When_contact_is_not_found()
    {
        // Arrange
        var contactId = testFixture.ContactId.ToString();
        var url = $"/api/v1/contact/{contactId}";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        Assert.Equal("404", actualResponse.Code);
        Assert.Equal("Contact not found", actualResponse.Message);
    }
    
    [Fact]
    public async Task Should_return_404_When_fund_is_not_found()
    {
        // Arrange
        var fundId = Guid.NewGuid();
        var url = $"/api/v1/fund/{fundId}/contacts";

        // Act
        using var httpResponse = await Client.GetAsync(url);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        Assert.Equal("404", actualResponse.Code);
        Assert.Equal("Fund not found", actualResponse.Message);
    }
    
    [Fact]
    public async Task Should_return_400_When_create_is_called_with_bad_inputs()
    {
        // Arrange
        const string url = "/api/v1/contact";
        using var request = new StringContent(
            new Contact
            {
                Name = "",
                Email = "test",
                PhoneNumber = "Phone"
            }.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Act
        using var httpResponse = await Client.PostAsync(url, request);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        Assert.Equal("400", actualResponse.Code);
        Assert.Contains("Name", actualResponse.Message);
        Assert.Contains("Email", actualResponse.Message);
        Assert.Contains("PhoneNumber", actualResponse.Message);
    }
    
    [Fact]
    public async Task Should_return_400_When_update_is_called_with_bad_inputs()
    {
        // Arrange
        var contactId = Guid.NewGuid().ToString();
        var url = $"/api/v1/contact/{contactId}";
        using var request = new StringContent(
            new Contact
            {
                Name = "",
                Email = "test",
                PhoneNumber = "0123456789"
            }.ToJson(),
            Encoding.UTF8,
            "application/json");

        // Act
        using var httpResponse = await Client.PutAsync(url, request);
        var actualResponse = await httpResponse.Content.ReadFromJsonAsync<ErrorResponse>();

        // Assert
        Assert.NotNull(actualResponse);
        Assert.Equal(HttpStatusCode.BadRequest, httpResponse.StatusCode);
        Assert.Equal("400", actualResponse.Code);
        Assert.Contains("Name", actualResponse.Message);
        Assert.Contains("Email", actualResponse.Message);
        Assert.DoesNotContain("PhoneNumber", actualResponse.Message);
    }
}
