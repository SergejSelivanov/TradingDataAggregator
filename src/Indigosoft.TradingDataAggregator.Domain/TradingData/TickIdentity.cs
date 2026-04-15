namespace Indigosoft.TradingDataAggregator.Domain.TradingData;

/// <summary>
/// Represents the natural identity of a tick for in-memory deduplication.
/// </summary>
public readonly record struct TickIdentity(
    InstrumentSymbol Symbol,
    Price Price,
    Volume Volume,
    DateTime TimestampUtc,
    TradingDataSourceName Source);
