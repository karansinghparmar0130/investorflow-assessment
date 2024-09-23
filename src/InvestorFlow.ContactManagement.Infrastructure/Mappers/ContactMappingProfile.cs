using AutoMapper;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;
using DomainFund = InvestorFlow.ContactManagement.Domain.Entities.Fund;

namespace InvestorFlow.ContactManagement.Infrastructure.Mappers;

/// <summary>
/// Automapper profile for contact transformations between Application and Infrastructure
/// </summary>
public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        // Domain.Entities to Infra Models Maps
        CreateMap<DomainContact, Contact>()
            .ForMember(dest => dest.ExternalId,
                opt =>
                    opt.MapFrom(src => src.ContactId));
        CreateMap<DomainFund, Fund>()
            .ForMember(dest => dest.ExternalId,
                opt =>
                    opt.MapFrom(src => src.FundId));
        
        // Infra Models to Domain.Entities Maps
        CreateMap<Contact, DomainContact>()
            .ForMember(dest => dest.ContactId,
                opt =>
                    opt.MapFrom(src => src.ExternalId));
        CreateMap<Fund, DomainFund>()
            .ForMember(dest => dest.FundId,
                opt =>
                    opt.MapFrom(src => src.ExternalId));
    }
}
