using InvestorFlow.ContactManagement.Domain.Entities;

namespace InvestorFlow.ContactManagement.Application.Interfaces;

/// <summary>
/// Exposes fund related service functionality
/// </summary>
public interface IFundService
{
    Task<IEnumerable<Contact>> ListContactsForFundAsync(Guid fundId);
}
