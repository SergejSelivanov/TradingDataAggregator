namespace Indigosoft.TradingDataAggregator.Domain.TradingData;

/// <summary>
/// Represents a validated market data tick in the application domain.
/// </summary>
public sealed class MarketTick
{
    private MarketTick(
        InstrumentSymbol symbol,
        Price price,
        Volume volume,
        DateTime timestampUtc,
        TradingDataSourceName source)
    {
        Symbol = symbol;
        Price = price;
        Volume = volume;
        TimestampUtc = timestampUtc;
        Source = source;
    }

    /// <summary>
    /// Gets the traded instrument symbol.
    /// </summary>
    public InstrumentSymbol Symbol { get; }

    /// <summary>
    /// Gets the tick price.
    /// </summary>
    public Price Price { get; }

    /// <summary>
    /// Gets the tick volume.
    /// </summary>
    public Volume Volume { get; }

    /// <summary>
    /// Gets the event timestamp in UTC.
    /// </summary>
    public DateTime TimestampUtc { get; }

    /// <summary>
    /// Gets the source exchange name.
    /// </summary>
    public TradingDataSourceName Source { get; }

    /// <summary>
    /// Creates a validated market tick.
    /// </summary>
    public static MarketTick Create(
        string symbol,
        decimal price,
        decimal volume,
        DateTime timestampUtc,
        string source)
    {
        if (timestampUtc == default)
            throw new ArgumentException("Tick timestamp cannot be empty.", nameof(timestampUtc));

        return new MarketTick(
            new InstrumentSymbol(symbol),
            new Price(price),
            new Volume(volume),
            DateTime.SpecifyKind(timestampUtc, DateTimeKind.Utc),
            new TradingDataSourceName(source));
    }

    /// <summary>
    /// Gets the stable identity used for short-window deduplication.
    /// </summary>
    public TickIdentity GetIdentity() => new(Symbol, Price, Volume, TimestampUtc, Source);
}
