using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Application.Deduplication;
using Indigosoft.TradingDataAggregator.Application.Ingestion;
using Indigosoft.TradingDataAggregator.Application.Options;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Application.Extensions;

/// <summary>
/// Registers Trading Data Aggregator application services.
/// </summary>
public static class TradingDataAggregatorApplicationExtensions
{
    /// <summary>
    /// Adds Trading Data Aggregator ingestion orchestration and processing services.
    /// </summary>
    public static IServiceCollection AddTradingDataAggregatorApplication(this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<IngestionOptions>>().Value;
            return Channel.CreateBounded<MarketTick>(new BoundedChannelOptions(options.ChannelCapacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            });
        });

        services.AddSingleton<ITickDeduplicationService, TickDeduplicationService>();
        services.AddHostedService<TradingDataIngestionBackgroundService>();
        services.AddHostedService<TradingDataProcessorBackgroundService>();

        return services;
    }
}
