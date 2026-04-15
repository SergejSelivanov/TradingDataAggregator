namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Common;

/// <summary>
/// Provides the database identifier for persistence entities.
/// </summary>
public abstract class Entity<TKey> : IEntity<TKey>
{
    /// <inheritdoc />
    public TKey Id { get; set; } = default!;
}
