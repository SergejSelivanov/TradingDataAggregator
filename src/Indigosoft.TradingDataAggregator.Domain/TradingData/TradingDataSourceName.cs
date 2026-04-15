namespace Indigosoft.TradingDataAggregator.Domain.TradingData;

/// <summary>
/// Represents the name of a market data source.
/// </summary>
public readonly record struct TradingDataSourceName
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TradingDataSourceName"/> struct.
    /// </summary>
    public TradingDataSourceName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Market data source name cannot be empty.", nameof(value));

        Value = value.Trim();
    }

    /// <summary>
    /// Gets the source name.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}
