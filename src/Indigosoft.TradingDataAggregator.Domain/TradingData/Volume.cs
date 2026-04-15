namespace Indigosoft.TradingDataAggregator.Domain.TradingData;

/// <summary>
/// Represents a positive traded volume.
/// </summary>
public readonly record struct Volume
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Volume"/> struct.
    /// </summary>
    public Volume(decimal value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Volume must be greater than zero.");

        Value = value;
    }

    /// <summary>
    /// Gets the decimal volume value.
    /// </summary>
    public decimal Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
