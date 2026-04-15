namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Common;

/// <summary>
/// Provides common fields for persistence entities created by the service.
/// </summary>
public abstract class CreatedEntity<TKey> : Entity<TKey>, ICreatedAtUtc
{
    /// <inheritdoc />
    public DateTime CreatedAtUtc { get; set; }
}
