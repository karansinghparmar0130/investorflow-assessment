using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Domain.Entities;
using InvestorFlow.ContactManagement.Domain.Exceptions;

namespace InvestorFlow.ContactManagement.Application.Services;

/// <summary>
/// Orchestrates calls for contact manipulation
/// </summary>
/// <param name="contactRepository">Contact Repository Instance</param>
/// <param name="fundRepository">Fund Repository Instance</param>
public class ContactService(
    IContactRepository contactRepository,
    IFundRepository fundRepository) : IContactService
{
    public async Task<Contact> GetContactAsync(Guid contactId)
    {
        var contactById = await contactRepository.GetAsync(contactId);
        if (contactById is null) throw new ContactNotFoundException();

        return contactById;
    }
    
    public async Task<Contact> CreateContactAsync(Contact contact)
    {
        // Create and assign guid
        contact.ContactId = Guid.NewGuid();
        return await contactRepository.AddAsync(contact);
    }

    public async Task<Contact> UpdateContactAsync(Contact contact, Guid contactId)
    {
        var contactById = await contactRepository.GetAsync(contactId);
        if (contactById is null) throw new ContactNotFoundException();

        // Assign values
        contactById.Name = contact.Name;
        contactById.Email = contact.Email;
        contactById.PhoneNumber = contact.PhoneNumber;
        
        return await contactRepository.UpdateAsync(contactById);
    }

    public async Task<Contact> AssignContactToFundAsync(Guid contactId, Guid fundId)
    {
        var contactById = await contactRepository.GetAsync(contactId);
        if (contactById is null) throw new ContactNotFoundException();

        var fundById = await fundRepository.GetAsync(fundId);
        if (fundById is null) throw new FundNotFoundException();

        if (fundById.Contacts.Any(contact => contact.ContactId == contactId))
            throw new ValidationException("Contact is already assigned to this fund");

        // Assign fund
        contactById.Fund = fundById;
        
        return await contactRepository.UpdateAsync(contactById);
    }

    public async Task<Contact> RemoveContactFromFundAsync(Guid contactId)
    {
        var contactById = await contactRepository.GetAsync(contactId);
        if (contactById is null) throw new ContactNotFoundException();

        // Remove fund
        contactById.Fund = null;
        
        return await contactRepository.UpdateAsync(contactById);
    }

    public async Task DeleteContactAsync(Guid contactId)
    {
        var contactById = await contactRepository.GetAsync(contactId);
        if (contactById is null) throw new ContactNotFoundException();
        if (contactById.Fund is not null) throw new ValidationException("Contact is assigned to a fund");
        
        await contactRepository.DeleteAsync(contactId);
    }
}
