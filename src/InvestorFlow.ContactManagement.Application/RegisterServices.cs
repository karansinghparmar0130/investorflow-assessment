using InvestorFlow.ContactManagement.Application.Interfaces;
using InvestorFlow.ContactManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorFlow.ContactManagement.Application;

public static class RegisterServices
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<IFundService, FundService>();
    }
}
