namespace InvestorFlow.ContactManagement.Domain.Exceptions;

public class FundNotFoundException : Exception
{
    public FundNotFoundException()
    {
    }

    public FundNotFoundException(string message)
        : base(message)
    {
    }

    public FundNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
