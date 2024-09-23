using AutoMapper;
using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using DomainFund = InvestorFlow.ContactManagement.Domain.Entities.Fund;

namespace InvestorFlow.ContactManagement.Infrastructure.Mappers;

/// <summary>
/// Mapper definition for fund transformations between Application and Infrastructure
/// </summary>
public class FundMapper(IMapper mapper): IMapper<Fund, DomainFund>
{
    public DomainFund Map(Fund source) =>
        mapper.Map<DomainFund>(source);
}
