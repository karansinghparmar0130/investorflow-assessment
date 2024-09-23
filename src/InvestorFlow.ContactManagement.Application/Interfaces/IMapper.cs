namespace InvestorFlow.ContactManagement.Application.Interfaces;

/// <summary>
/// Generic Mapper interface to facilitate abstraction
/// </summary>
/// <typeparam name="TSrc">source type</typeparam>
/// <typeparam name="TResult">destination type</typeparam>
public interface IMapper<in TSrc, out TResult>
    where TSrc : class
    where TResult : class
{
    TResult Map(TSrc source);
}
