namespace Indigosoft.TradingDataAggregator.Domain.TradingData;

/// <summary>
/// Represents a normalized instrument symbol used across all market data sources.
/// </summary>
public readonly record struct InstrumentSymbol
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InstrumentSymbol"/> struct.
    /// </summary>
    public InstrumentSymbol(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Instrument symbol cannot be empty.", nameof(value));

        Value = value.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Gets the normalized symbol value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value;
}
