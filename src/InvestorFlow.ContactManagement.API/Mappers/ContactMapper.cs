using AutoMapper;
using InvestorFlow.ContactManagement.API.Models;
using InvestorFlow.ContactManagement.Application.Interfaces;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.API.Mappers;

/// <summary>
/// Mapper definition for contact transformations between API and Application
/// </summary>
public class ContactMapper(IMapper mapper) :
    IMapper<Contact, DomainContact>,
    IMapper<DomainContact, ContactResponse>,
    IMapper<IEnumerable<DomainContact>, FundResponse>
{
    public DomainContact Map(Contact source) =>
        mapper.Map<DomainContact>(source);

    public ContactResponse Map(DomainContact source) =>
        mapper.Map<ContactResponse>(source);

    public FundResponse Map(IEnumerable<DomainContact> source)
    {
        var response = new FundResponse();
        response.AddRange(source.Select(mapper.Map<ContactResponse>));

        return response;
    }
}
