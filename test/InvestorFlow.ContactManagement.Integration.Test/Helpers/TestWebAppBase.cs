using InvestorFlow.ContactManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InvestorFlow.ContactManagement.Integration.Test.Helpers;

public abstract class TestWebAppBase: 
    IClassFixture<TestWebAppFactory>
{
    protected readonly HttpClient Client;

    protected TestWebAppBase(TestWebAppFactory factory)
    {
        // Migrate Db
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();

        // Http client for test
        Client = factory.CreateClient();
    }
}
