using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Domain.Exceptions;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using DomainFund = InvestorFlow.ContactManagement.Domain.Entities.Fund;

namespace InvestorFlow.ContactManagement.Infrastructure.Persistence;

/// <summary>
/// Implements fund related repository functionality
/// </summary>
public class FundRepository(
    AppDbContext dbContext,
    IMapper<Fund, DomainFund> fundResponseMapper) : IFundRepository
{
    public async Task<DomainFund> GetAsync(Guid fundId)
    {
        var fundById = await dbContext
            .Funds
            .AsNoTracking()
            .Include(fund => fund.Contacts)
            .FirstOrDefaultAsync(fund => fund.ExternalId == fundId);

        if (fundById is null)
            throw new FundNotFoundException();

        return fundResponseMapper.Map(fundById);
    }
}
