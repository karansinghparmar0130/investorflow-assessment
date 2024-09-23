namespace InvestorFlow.ContactManagement.Infrastructure.Models;

/// <summary>
/// Contact Entity used with Database
/// </summary>
public class Contact
{
    public int Id { get; set; } // Auto-incremented primary key
    public required Guid ExternalId { get; set; } // External Identifier for consumers, should be generated and passed
    public required string Name { get; set; } // Mandatory
    public string? Email { get; set; } // Optional
    public string? PhoneNumber { get; set; } // Optional

    // Foreign Key
    // Nullable to indicate that a contact may not be assigned to a fund
    // This references Id from Fund
    public int? FundId { get; set; } 
    
    // Navigation property to the Fund
    // Contact can have only one fund
    public Fund? Fund { get; set; }
}
