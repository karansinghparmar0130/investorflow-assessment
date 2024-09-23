using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using AutoMapper;
using InvestorFlow.ContactManagement.API;
using InvestorFlow.ContactManagement.API.Middlewares;
using InvestorFlow.ContactManagement.Application;
using InvestorFlow.ContactManagement.Infrastructure;
using Serilog;

// Create
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(builder.Configuration)
    .CreateBootstrapLogger();

Log.Information("Starting up");

// Configure
BuildConfiguration();
ConfigureServices();

// Build
var app = builder.Build();
    
// Extend
ConfigureMiddlewares();

try
{
    // Run
    app.Run();
}
catch (Exception ex)
{
    // Log bootstrapping errors
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    throw;
}
finally
{
    // Ensure to flush and stop
   Log.CloseAndFlush();
}

#region Method Definitions

void BuildConfiguration()
{
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog();
}

void ConfigureServices()
{
    // Add framework services
    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    }).AddMvc();
    builder.Services.AddHealthChecks();
    builder.Services.AddSingleton(_ =>
        new MapperConfiguration(configure =>
            {
                configure.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            })
            .CreateMapper());

    // Add Dependencies
    builder.Services.AddApi();
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
}

void ConfigureMiddlewares()
{
    // Ordering here is important
    app.MapHealthChecks("/health");
    app.MapControllers();
    app.UseMiddleware<CorrelationMiddleware>();
    app.UseMiddleware<GlobalExceptionMiddleware>();
}

#endregion
