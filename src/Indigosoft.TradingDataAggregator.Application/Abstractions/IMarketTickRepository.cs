using Indigosoft.TradingDataAggregator.Domain.TradingData;

namespace Indigosoft.TradingDataAggregator.Application.Abstractions;

/// <summary>
/// Persists accepted market ticks.
/// </summary>
public interface IMarketTickRepository
{
    /// <summary>
    /// Adds accepted market ticks to the current persistence batch.
    /// </summary>
    Task AddRangeAsync(IReadOnlyCollection<MarketTick> ticks, CancellationToken cancellationToken);

    /// <summary>
    /// Persists all pending changes in the current persistence batch.
    /// </summary>
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
