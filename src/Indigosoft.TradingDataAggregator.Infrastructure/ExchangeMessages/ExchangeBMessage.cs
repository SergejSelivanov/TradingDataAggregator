namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;

/// <summary>
/// Represents the raw message format produced by mock exchange B.
/// </summary>
public sealed record ExchangeBMessage(
    string Instrument,
    string Price,
    string Size,
    DateTime Time);
