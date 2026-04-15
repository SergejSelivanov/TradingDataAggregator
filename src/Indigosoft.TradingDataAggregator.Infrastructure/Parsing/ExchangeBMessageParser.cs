using System.Globalization;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

/// <summary>
/// Parses mock exchange B messages into domain market ticks.
/// </summary>
public sealed class ExchangeBMessageParser : IExchangeMessageParser<ExchangeBMessage>
{
    /// <inheritdoc />
    public MarketTick Parse(ExchangeBMessage message)
    {
        return MarketTick.Create(
            NormalizeSymbol(message.Instrument),
            decimal.Parse(message.Price, CultureInfo.InvariantCulture),
            decimal.Parse(message.Size, CultureInfo.InvariantCulture),
            DateTime.SpecifyKind(message.Time, DateTimeKind.Utc),
            "MockExchangeB");
    }

    private static string NormalizeSymbol(string instrument)
    {
        return instrument.Length == 6
            ? string.Concat(instrument.AsSpan(0, 3), "-", instrument.AsSpan(3, 3))
            : instrument;
    }
}
