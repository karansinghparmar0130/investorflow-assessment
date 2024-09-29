using InvestorFlow.ContactManagement.Domain.Entities;

namespace InvestorFlow.ContactManagement.Application.Interfaces;

/// <summary>
/// Exposes fund related repository functionality
/// </summary>
public interface IFundRepository
{
    Task<Fund?> GetAsync(Guid fundId, bool includeContacts = false);
}
