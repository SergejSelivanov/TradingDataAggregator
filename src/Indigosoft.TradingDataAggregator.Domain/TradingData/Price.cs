namespace Indigosoft.TradingDataAggregator.Domain.TradingData;

/// <summary>
/// Represents a positive market price.
/// </summary>
public readonly record struct Price
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Price"/> struct.
    /// </summary>
    public Price(decimal value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Price must be greater than zero.");

        Value = value;
    }

    /// <summary>
    /// Gets the decimal price value.
    /// </summary>
    public decimal Value { get; }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
