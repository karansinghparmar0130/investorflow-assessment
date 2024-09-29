using InvestorFlow.ContactManagement.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace InvestorFlow.ContactManagement.Integration.Test.Helpers;

public class TestWebAppFactory :
    WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private const string Database = "ContactManagementDb";
    private const string Username = "sa";
    private const string Password = "Strong_password_123!";
    private const ushort MsSqlPort = 1433;

    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword(Password)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var host = _dbContainer.Hostname;
        var port = _dbContainer.GetMappedPublicPort(MsSqlPort);

        var connectionString =
            $"Server={host},{port};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=True";

        builder.ConfigureTestServices(services =>
        {
            // Remove existing if already setup
            var descriptorType =
                typeof(DbContextOptions<AppDbContext>);

            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == descriptorType);

            if (descriptor is not null)
                services.Remove(descriptor);

            // Setup Db context to posing to test container
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        });
    }

    public Task InitializeAsync() => _dbContainer.StartAsync();

    public new Task DisposeAsync() => _dbContainer.StopAsync();
}
