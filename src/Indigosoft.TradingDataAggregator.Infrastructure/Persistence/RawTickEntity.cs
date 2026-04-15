using Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Common;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence;

/// <summary>
/// Stores a normalized raw market tick exactly as accepted by the application pipeline.
/// </summary>
public sealed class RawTickEntity : CreatedEntity<long>
{
    /// <summary>
    /// Gets or sets the traded instrument symbol.
    /// </summary>
    public required string Symbol { get; set; }

    /// <summary>
    /// Gets or sets the tick price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the tick volume.
    /// </summary>
    public decimal Volume { get; set; }

    /// <summary>
    /// Gets or sets the tick timestamp in UTC.
    /// </summary>
    public DateTime TimestampUtc { get; set; }

    /// <summary>
    /// Gets or sets the exchange source.
    /// </summary>
    public required string Source { get; set; }

}
