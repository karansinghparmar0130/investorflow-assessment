using InvestorFlow.ContactManagement.API.Mappers;
using InvestorFlow.ContactManagement.API.Models;
using InvestorFlow.ContactManagement.Application.Interfaces;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.API;

public static class RegisterServices
{
    public static void AddApi(this IServiceCollection services)
    {
        // Mappers
        services.AddSingleton<IMapper<Contact, DomainContact>, ContactMapper>();
        services.AddSingleton<IMapper<DomainContact, ContactResponse>, ContactMapper>();
        services.AddSingleton<IMapper<IEnumerable<DomainContact>, FundResponse>, ContactMapper>();
    }
}
