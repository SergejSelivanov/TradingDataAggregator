namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Common;

/// <summary>
/// Represents a persistence entity with a creation timestamp.
/// </summary>
public interface ICreatedAtUtc
{
    /// <summary>
    /// Gets or sets the creation timestamp in UTC.
    /// </summary>
    DateTime CreatedAtUtc { get; set; }
}
