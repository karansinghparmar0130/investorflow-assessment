using AutoMapper;
using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.Infrastructure.Mappers;

/// <summary>
/// Mapper definition for contact transformations between Application and Infrastructure
/// </summary>
public class ContactMapper(IMapper mapper) :
    IMapper<Contact, DomainContact>,
    IMapper<DomainContact, Contact>
{
    public DomainContact Map(Contact source) =>
        mapper.Map<DomainContact>(source);

    public Contact Map(DomainContact source) =>
        mapper.Map<Contact>(source);
}
