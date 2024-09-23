namespace InvestorFlow.ContactManagement.Domain.Entities;

public class Contact
{
    public int Id { get; set; } // Added for tracking only
    public Guid? ContactId { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Fund? Fund { get; set; }
}
