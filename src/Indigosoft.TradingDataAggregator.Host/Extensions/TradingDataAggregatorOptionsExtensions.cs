using Indigosoft.TradingDataAggregator.Application.Options;
using Indigosoft.TradingDataAggregator.Infrastructure.Streaming;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Indigosoft.TradingDataAggregator.Host.Extensions;

/// <summary>
/// Registers host-level options from configuration.
/// </summary>
public static class TradingDataAggregatorOptionsExtensions
{
    /// <summary>
    /// Binds Trading Data Aggregator options from configuration sections.
    /// </summary>
    public static IServiceCollection ConfigureTradingDataAggregatorOptions(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<IngestionOptions>()
            .Bind(configuration.GetSection("Ingestion"));

        services.AddOptions<ReconnectOptions>()
            .Bind(configuration.GetSection("Reconnect"));

        return services;
    }
}
