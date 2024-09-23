using AutoMapper;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using DomainFund = InvestorFlow.ContactManagement.Domain.Entities.Fund;

namespace InvestorFlow.ContactManagement.Infrastructure.Mappers;

/// <summary>
/// Automapper profile for fund transformations between Application and Infrastructure
/// </summary>
public class FundMappingProfile : Profile
{
    public FundMappingProfile()
    {
        // Infra Models to Domain.Entities Maps
        CreateMap<Fund, DomainFund>()
            .ForMember(dest => dest.FundId,
                opt =>
                    opt.MapFrom(src => src.ExternalId));
    }
}
