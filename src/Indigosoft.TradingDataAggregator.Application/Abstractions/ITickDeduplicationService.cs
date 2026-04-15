using Indigosoft.TradingDataAggregator.Domain.TradingData;

namespace Indigosoft.TradingDataAggregator.Application.Abstractions;

/// <summary>
/// Decides whether an incoming market tick should be accepted for processing.
/// </summary>
public interface ITickDeduplicationService
{
    /// <summary>
    /// Returns true when the tick has not been seen within the configured retention window.
    /// </summary>
    bool TryAccept(MarketTick tick);
}
