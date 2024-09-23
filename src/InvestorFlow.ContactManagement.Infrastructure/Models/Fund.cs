namespace InvestorFlow.ContactManagement.Infrastructure.Models;

/// <summary>
/// Fund Entity used with Database
/// </summary>
public class Fund
{
    public int Id { get; set; } // Auto-incremented primary key
    public required Guid ExternalId { get; set; } // External Identifier for consumers, should be generated and passed
    public required string Name { get; set; } // Mandatory

    // Navigation property
    // Fund can have multiple contacts
    public ICollection<Contact> Contacts { get; set; } = [];
}
