namespace InvestorFlow.ContactManagement.Integration.Test.Helpers;

public class TestFixture
{
    public Guid? ContactId { get; set; }
    public Guid? FundId { get; } = Guid.Parse("8c315b74-f063-48b0-a479-35763535075c"); // Based on seeded data
}
