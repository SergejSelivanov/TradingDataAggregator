using Indigosoft.TradingDataAggregator.Domain.TradingData;

namespace Indigosoft.TradingDataAggregator.Application.Abstractions;

/// <summary>
/// Streams normalized market ticks from a market data source.
/// </summary>
public interface ITradingDataSource
{
    /// <summary>
    /// Gets the market data source name.
    /// </summary>
    TradingDataSourceName Source { get; }

    /// <summary>
    /// Opens the source stream and yields validated market ticks.
    /// </summary>
    IAsyncEnumerable<MarketTick> StreamTicksAsync(CancellationToken cancellationToken);
}
