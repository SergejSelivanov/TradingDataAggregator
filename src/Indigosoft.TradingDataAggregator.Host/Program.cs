using Indigosoft.TradingDataAggregator.Application.Extensions;
using Indigosoft.TradingDataAggregator.Infrastructure.Persistence;
using Indigosoft.TradingDataAggregator.Host.Extensions;
using Indigosoft.TradingDataAggregator.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using var host = new HostBuilder()
    .UseContentRoot(Directory.GetCurrentDirectory())
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
        configuration.AddEnvironmentVariables();
        configuration.AddCommandLine(args);
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.AddConfiguration(context.Configuration.GetSection("Logging"));
        logging.AddConsole();
    })
    .ConfigureServices((context, services) => services
        .ConfigureTradingDataAggregatorOptions(context.Configuration)
        .AddTradingDataAggregatorApplication()
        .AddTradingDataAggregatorInfrastructure(context.Configuration))
    .UseConsoleLifetime()
    .Build();

await TradingDataDatabaseInitializer.EnsureDatabaseCreatedAsync(host.Services);
await host.RunAsync();
