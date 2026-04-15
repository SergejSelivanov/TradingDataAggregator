using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

/// <summary>
/// Parses mock exchange C messages into domain market ticks.
/// </summary>
public sealed class ExchangeCMessageParser : IExchangeMessageParser<ExchangeCMessage>
{
    /// <inheritdoc />
    public MarketTick Parse(ExchangeCMessage message) =>
        MarketTick.Create(
            $"{message.BaseAsset}-{message.QuoteAsset}",
            message.Bid,
            message.Amount,
            message.CreatedAt.UtcDateTime,
            "MockExchangeC");
}
