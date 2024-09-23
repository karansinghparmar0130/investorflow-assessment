namespace InvestorFlow.ContactManagement.Domain.Entities;

public class Fund
{
    public int Id { get; set; } // Added for tracking only
    public required Guid? FundId { get; set; }
    public required string Name { get; set; }
    public IEnumerable<Contact> Contacts { get; set; } = [];
}
