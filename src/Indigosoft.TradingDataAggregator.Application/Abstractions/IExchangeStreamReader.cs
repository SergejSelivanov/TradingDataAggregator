using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Domain.TradingData;

namespace Indigosoft.TradingDataAggregator.Application.Abstractions;

/// <summary>
/// Reads a market data source into the application ingestion channel.
/// </summary>
public interface IExchangeStreamReader
{
    /// <summary>
    /// Reads ticks from the source and writes them into the output channel.
    /// </summary>
    Task ReadAsync(
        ITradingDataSource source,
        ChannelWriter<MarketTick> output,
        CancellationToken cancellationToken);
}
