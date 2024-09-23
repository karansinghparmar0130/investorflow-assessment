using InvestorFlow.ContactManagement.Domain.Entities;

namespace InvestorFlow.ContactManagement.Application.Interfaces;

/// <summary>
/// Exposes contact related service functionality
/// </summary>
public interface IContactService
{
    Task<Contact> GetContactAsync(Guid contactId);
    Task<Contact> CreateContactAsync(Contact contact);
    Task<Contact> UpdateContactAsync(Contact contact, Guid contactId);
    Task<Contact> AssignContactToFundAsync(Guid contactId, Guid fundId);
    Task<Contact> RemoveContactFromFundAsync(Guid contactId);
    Task DeleteContactAsync(Guid contactId);
}
