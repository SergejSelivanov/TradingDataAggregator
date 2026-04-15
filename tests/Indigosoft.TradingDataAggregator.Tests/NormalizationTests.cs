using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;
using Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

namespace Indigosoft.TradingDataAggregator.Tests;

/// <summary>
/// Verifies exchange-specific tick normalization.
/// </summary>
public sealed class NormalizationTests
{
    /// <summary>
    /// Ensures exchange A messages are mapped to the domain model.
    /// </summary>
    [Fact]
    public void ExchangeAMessage_is_normalized()
    {
        var parser = new ExchangeAMessageParser();
        var message = new ExchangeAMessage("BTC-USD", 42_000.12m, 1.25m, 1_700_000_000_000);

        var tick = parser.Parse(message);

        Assert.Equal("BTC-USD", tick.Symbol.Value);
        Assert.Equal(42_000.12m, tick.Price.Value);
        Assert.Equal(1.25m, tick.Volume.Value);
        Assert.Equal(DateTimeOffset.FromUnixTimeMilliseconds(1_700_000_000_000).UtcDateTime, tick.TimestampUtc);
        Assert.Equal("MockExchangeA", tick.Source.Value);
    }

    /// <summary>
    /// Ensures exchange B messages normalize compact symbols and decimal strings.
    /// </summary>
    [Fact]
    public void ExchangeBMessage_is_normalized()
    {
        var parser = new ExchangeBMessageParser();
        var timestamp = new DateTime(2026, 4, 15, 12, 0, 0, DateTimeKind.Utc);
        var message = new ExchangeBMessage("ETHUSD", "3200.50", "0.75", timestamp);

        var tick = parser.Parse(message);

        Assert.Equal("ETH-USD", tick.Symbol.Value);
        Assert.Equal(3_200.50m, tick.Price.Value);
        Assert.Equal(0.75m, tick.Volume.Value);
        Assert.Equal(timestamp, tick.TimestampUtc);
        Assert.Equal("MockExchangeB", tick.Source.Value);
    }

    /// <summary>
    /// Ensures exchange C asset fields are converted to a single symbol.
    /// </summary>
    [Fact]
    public void ExchangeCMessage_is_normalized()
    {
        var parser = new ExchangeCMessageParser();
        var timestamp = new DateTimeOffset(2026, 4, 15, 12, 0, 0, TimeSpan.Zero);
        var message = new ExchangeCMessage("SOL", "USD", 150.10m, 4.5m, timestamp);

        var tick = parser.Parse(message);

        Assert.Equal("SOL-USD", tick.Symbol.Value);
        Assert.Equal(150.10m, tick.Price.Value);
        Assert.Equal(4.5m, tick.Volume.Value);
        Assert.Equal(timestamp.UtcDateTime, tick.TimestampUtc);
        Assert.Equal("MockExchangeC", tick.Source.Value);
    }
}
