using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

/// <summary>
/// Parses mock exchange A messages into domain market ticks.
/// </summary>
public sealed class ExchangeAMessageParser : IExchangeMessageParser<ExchangeAMessage>
{
    /// <inheritdoc />
    public MarketTick Parse(ExchangeAMessage message)
    {
        return MarketTick.Create(
            message.Pair,
            message.LastPrice,
            message.Quantity,
            DateTimeOffset.FromUnixTimeMilliseconds(message.EventTimeUnixMilliseconds).UtcDateTime,
            "MockExchangeA");
    }
}
