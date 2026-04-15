using Indigosoft.TradingDataAggregator.Application.Deduplication;
using Indigosoft.TradingDataAggregator.Application.Options;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Tests;

/// <summary>
/// Verifies in-memory market tick deduplication.
/// </summary>
public sealed class DeduplicationTests
{
    /// <summary>
    /// Ensures an identical market tick is accepted only once.
    /// </summary>
    [Fact]
    public void TryAccept_rejects_duplicate_tick()
    {
        var policy = CreatePolicy();
        var tick = CreateTick("MockExchangeA");

        Assert.True(policy.TryAccept(tick));
        Assert.False(policy.TryAccept(tick));
    }

    /// <summary>
    /// Ensures similar ticks from different sources are kept.
    /// </summary>
    [Fact]
    public void TryAccept_keeps_same_tick_from_different_sources()
    {
        var policy = CreatePolicy();

        Assert.True(policy.TryAccept(CreateTick("MockExchangeA")));
        Assert.True(policy.TryAccept(CreateTick("MockExchangeB")));
    }

    /// <summary>
    /// Ensures an identical market tick is accepted again after the retention window expires.
    /// </summary>
    [Fact]
    public void TryAccept_accepts_duplicate_after_retention_period()
    {
        var timeProvider = new ManualTimeProvider(new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero));
        var policy = CreatePolicy(
            new IngestionOptions { DeduplicationRetentionPeriod = TimeSpan.FromMinutes(5) },
            timeProvider);
        var tick = CreateTick("MockExchangeA");

        Assert.True(policy.TryAccept(tick));

        timeProvider.Advance(TimeSpan.FromMinutes(4));
        Assert.False(policy.TryAccept(tick));

        timeProvider.Advance(TimeSpan.FromMinutes(2));
        Assert.True(policy.TryAccept(tick));
    }

    private static TickDeduplicationService CreatePolicy() => new(Options.Create(new IngestionOptions()));

    private static TickDeduplicationService CreatePolicy(IngestionOptions options, TimeProvider timeProvider) =>
        new(Options.Create(options), timeProvider);

    private static MarketTick CreateTick(string source) =>
        MarketTick.Create(
            "BTC-USD",
            42_000m,
            1.5m,
            new DateTime(2026, 4, 15, 12, 0, 0, DateTimeKind.Utc),
            source);

    private sealed class ManualTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        private DateTimeOffset _utcNow = utcNow;

        public override DateTimeOffset GetUtcNow() => _utcNow;

        public void Advance(TimeSpan timeSpan) => _utcNow += timeSpan;
    }
}
