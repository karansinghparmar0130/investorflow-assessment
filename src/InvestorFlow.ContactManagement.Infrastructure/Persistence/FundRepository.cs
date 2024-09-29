using InvestorFlow.ContactManagement.Application.Interfaces;
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
    public async Task<DomainFund?> GetAsync(Guid fundId, bool includeContacts = false)
    {
        var query = dbContext
            .Funds
            .AsNoTracking()
            .AsQueryable();

        // Conditionally include Contacts based on includeContacts parameter
        if (includeContacts)
            query = query.Include(fund => fund.Contacts);

        var fundById = await query
            .FirstOrDefaultAsync(fund => fund.ExternalId == fundId);

        return fundById is null
            ? null
            : fundResponseMapper.Map(fundById);
    }
}
