namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Common;

/// <summary>
/// Represents a persistence entity with a database identifier.
/// </summary>
public interface IEntity<TKey>
{
    /// <summary>
    /// Gets or sets the database identifier.
    /// </summary>
    TKey Id { get; set; }
}
