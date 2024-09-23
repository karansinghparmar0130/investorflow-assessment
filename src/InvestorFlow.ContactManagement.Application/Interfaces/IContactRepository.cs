using InvestorFlow.ContactManagement.Domain.Entities;

namespace InvestorFlow.ContactManagement.Application.Interfaces;

/// <summary>
/// Exposes contact related repository functionality
/// </summary>
public interface IContactRepository
{
    Task<Contact> AddAsync(Contact contact);
    Task<Contact> UpdateAsync(Contact contact);
    Task<Contact> GetAsync(Guid contactId);
    Task DeleteAsync(Guid contactId);
}
