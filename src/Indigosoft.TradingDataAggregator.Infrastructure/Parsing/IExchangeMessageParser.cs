using Indigosoft.TradingDataAggregator.Domain.TradingData;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

/// <summary>
/// Parses source-specific exchange messages into validated domain ticks.
/// </summary>
public interface IExchangeMessageParser<in TMessage>
{
    /// <summary>
    /// Converts a raw exchange message into a domain market tick.
    /// </summary>
    MarketTick Parse(TMessage message);
}
