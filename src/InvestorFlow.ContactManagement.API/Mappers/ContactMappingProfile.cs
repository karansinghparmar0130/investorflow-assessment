using AutoMapper;
using InvestorFlow.ContactManagement.API.Models;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.API.Mappers;

/// <summary>
/// Automapper profile for contact transformations between API and Application
/// </summary>
public class ContactMappingProfile: Profile
{
    public ContactMappingProfile()
    {
        // Models to Domain.Entities Maps
        CreateMap<Contact, DomainContact>();
        
        // Domain.Entities to Models Maps
        CreateMap<DomainContact, ContactResponse>()
            .ForMember(dest => dest.FundId,
                opt =>
                    opt.MapFrom(src => src.Fund != null
                        ? src.Fund.FundId
                        : null));
    }
}
