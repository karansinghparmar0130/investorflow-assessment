using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.Infrastructure.Persistence;

/// <summary>
/// Implements contact related repository functionality
/// </summary>
public class ContactRepository(
    AppDbContext dbContext,
    IMapper<DomainContact, Contact> contactMapper,
    IMapper<Contact, DomainContact> contactResponseMapper) : IContactRepository
{
    public async Task<DomainContact> AddAsync(DomainContact contact)
    {
        var contactEntity = contactMapper.Map(contact);

        dbContext
            .Contacts
            .Add(contactEntity);

        await dbContext.SaveChangesAsync();
        return contactResponseMapper.Map(contactEntity);
    }

    public async Task<DomainContact> UpdateAsync(DomainContact contact)
    {
        var contactEntity = contactMapper.Map(contact);

        dbContext
            .Contacts
            .Update(contactEntity);

        await dbContext.SaveChangesAsync();
        return contactResponseMapper.Map(contactEntity);
    }

    public async Task<DomainContact?> GetAsync(Guid contactId)
    {
        var contactById = await dbContext
            .Contacts
            .AsNoTracking()
            .Include(contact => contact.Fund)
            .FirstOrDefaultAsync(contact => contact.ExternalId == contactId);

        return contactById is null
            ? null
            : contactResponseMapper.Map(contactById);
    }

    public async Task DeleteAsync(Guid contactId)
    {
        await dbContext
            .Contacts
            .Where(contact => contact.ExternalId == contactId)
            .ExecuteDeleteAsync();
    }
}
