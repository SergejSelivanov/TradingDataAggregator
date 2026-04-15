namespace Indigosoft.TradingDataAggregator.Application.Options;

/// <summary>
/// Defines application-level ingestion pipeline settings.
/// </summary>
public sealed class IngestionOptions
{
    /// <summary>
    /// Gets or sets the bounded channel capacity.
    /// </summary>
    public int ChannelCapacity { get; set; } = 5_000;

    /// <summary>
    /// Gets or sets the maximum number of ticks saved in one repository call.
    /// </summary>
    public int MaxBatchSize { get; set; } = 100;

    /// <summary>
    /// Gets or sets the maximum time a non-empty batch can wait before it is flushed.
    /// </summary>
    public TimeSpan FlushInterval { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets the retention period for in-memory deduplication keys.
    /// </summary>
    public TimeSpan DeduplicationRetentionPeriod { get; set; } = TimeSpan.FromMinutes(5);
}
