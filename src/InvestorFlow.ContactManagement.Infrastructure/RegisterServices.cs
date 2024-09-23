using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Infrastructure.Mappers;
using InvestorFlow.ContactManagement.Infrastructure.Models;
using InvestorFlow.ContactManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DomainFund = InvestorFlow.ContactManagement.Domain.Entities.Fund;
using DomainContact = InvestorFlow.ContactManagement.Domain.Entities.Contact;

namespace InvestorFlow.ContactManagement.Infrastructure;

public static class RegisterServices
{
    public static void AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Mappers
        services.AddSingleton<IMapper<DomainContact, Contact>, ContactMapper>();
        services.AddSingleton<IMapper<Contact, DomainContact>, ContactMapper>();
        services.AddSingleton<IMapper<Fund, DomainFund>, FundMapper>();
        
        // DB Context
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });
        
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IFundRepository, FundRepository>();
    }
}
