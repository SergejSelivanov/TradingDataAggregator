using System.Collections.Concurrent;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Application.Options;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Application.Deduplication;

/// <summary>
/// Applies bounded in-memory deduplication for recently accepted ticks.
/// </summary>
public sealed class TickDeduplicationService : ITickDeduplicationService
{
    private readonly ConcurrentDictionary<TickIdentity, DateTime> _seenTicks = new();
    private readonly TimeSpan _retentionPeriod;
    private long _acceptedTicks;

    /// <summary>
    /// Initializes a new instance of the <see cref="TickDeduplicationService"/> class.
    /// </summary>
    public TickDeduplicationService(IOptions<IngestionOptions> options) =>
        _retentionPeriod = options.Value.DeduplicationRetentionPeriod;

    /// <inheritdoc />
    public bool TryAccept(MarketTick tick)
    {
        CleanupIfNeeded();
        return _seenTicks.TryAdd(tick.GetIdentity(), DateTime.UtcNow);
    }

    private void CleanupIfNeeded()
    {
        var acceptedTicks = Interlocked.Increment(ref _acceptedTicks);
        if (acceptedTicks % 1_000 != 0)
            return;

        var threshold = DateTime.UtcNow - _retentionPeriod;
        foreach (var item in _seenTicks)
            if (item.Value < threshold)
                _seenTicks.TryRemove(item.Key, out _);
    }
}
