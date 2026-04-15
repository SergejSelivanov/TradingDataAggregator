namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;

/// <summary>
/// Represents the raw message format produced by mock exchange A.
/// </summary>
public sealed record ExchangeAMessage(
    string Pair,
    decimal LastPrice,
    decimal Quantity,
    long EventTimeUnixMilliseconds);
