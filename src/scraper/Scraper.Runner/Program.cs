using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scraper.ApiClient;
using Scraper.Runner;
using Scraper.Runner.Scraper;
using Serilog;

var host = AppStartup();
var scraper = host.Services.GetRequiredService<IScraperService>();
await scraper.RunAsync();

static IHost AppStartup()
{
    var builder = new ConfigurationBuilder();
    BuildConfig(builder);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Build())
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    Log.Logger.Information("Application Starting");

    var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) => {
            ConfigureServices(services, context.Configuration);
        })
        .UseSerilog()
        .Build();

    return host;
}

static void BuildConfig(IConfigurationBuilder builder)
{
    // Checks the current directory that the application is running on 
    // Then once the file 'appsetting.json' is found, we are adding it.
    // We add env variables, which can override the configs in appsettings.json
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();
}

static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    services.AddTransient<IScraperService, ScraperService>();

    services.AddTransient<IRickAndMortyApiClient, RickAndMortyApiClient>();

    services.AddInfrastructure(configuration);

    services.AddHttpClient<IRickAndMortyApiClient, RickAndMortyApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("RickAndMortyApiBaseUrl")!);
            client.Timeout = TimeSpan.FromMinutes(5);
        })
        .AddPolicyHandler(RetryPolicy.GetRetryPolicy());
}