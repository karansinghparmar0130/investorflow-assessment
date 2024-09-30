using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Domain.Exceptions;
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
            var host = Environment
                .GetEnvironmentVariable("MSSQL_HOST");
            if (string.IsNullOrWhiteSpace(host))
                throw new InfrastructureException("Server Host should be available for Database connection");
            
            var password = Environment
                .GetEnvironmentVariable("MSSQL_SA_PASSWORD");
            if (string.IsNullOrWhiteSpace(password))
                throw new InfrastructureException("Password should be available for Database connection");

            var connectionString = configuration.GetConnectionString("Default");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new InfrastructureException("Connection string should be available for Database connection");

            var completeConnectionString = string.Format(connectionString, host, password);
            options.UseSqlServer(completeConnectionString);
        });
        
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IFundRepository, FundRepository>();
    }
}
