namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;

/// <summary>
/// Represents the raw message format produced by mock exchange C.
/// </summary>
public sealed record ExchangeCMessage(
    string BaseAsset,
    string QuoteAsset,
    decimal Bid,
    decimal Amount,
    DateTimeOffset CreatedAt);
