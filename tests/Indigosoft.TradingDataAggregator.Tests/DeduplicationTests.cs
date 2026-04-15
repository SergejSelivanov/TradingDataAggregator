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

    private static TickDeduplicationService CreatePolicy() => new(Options.Create(new IngestionOptions()));

    private static MarketTick CreateTick(string source) =>
        MarketTick.Create(
            "BTC-USD",
            42_000m,
            1.5m,
            new DateTime(2026, 4, 15, 12, 0, 0, DateTimeKind.Utc),
            source);
}
