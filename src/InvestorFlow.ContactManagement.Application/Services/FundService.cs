using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Domain.Entities;
using InvestorFlow.ContactManagement.Domain.Exceptions;

namespace InvestorFlow.ContactManagement.Application.Services;

/// <summary>
/// Orchestrates calls for fund manipulation
/// </summary>
/// <param name="fundRepository">Fund Repository Instance</param>
public class FundService(IFundRepository fundRepository) : IFundService
{
    public async Task<IEnumerable<Contact>> ListContactsForFundAsync(Guid fundId)
    {
        var fundById = await fundRepository.GetAsync(fundId, includeContacts: true);
        if (fundById is null) throw new FundNotFoundException();

        return fundById.Contacts;
    }
}
