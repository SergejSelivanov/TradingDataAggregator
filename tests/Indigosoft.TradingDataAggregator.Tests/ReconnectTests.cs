using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;
using Indigosoft.TradingDataAggregator.Infrastructure.Streaming;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Tests;

/// <summary>
/// Verifies reconnect behavior for failing exchange streams.
/// </summary>
public sealed class ReconnectTests
{
    /// <summary>
    /// Ensures the reader reconnects after a websocket stream failure.
    /// </summary>
    [Fact]
    public async Task ReadAsync_reconnects_after_disconnect()
    {
        var source = new FailingOnceTradingDataSource();
        var channel = Channel.CreateUnbounded<MarketTick>();
        var reader = new ExchangeStreamReader(
            Options.Create(new ReconnectOptions { InitialDelay = TimeSpan.Zero, MaxDelay = TimeSpan.Zero }),
            NullLogger<ExchangeStreamReader>.Instance);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        var readTask = reader.ReadAsync(source, channel.Writer, cancellation.Token);
        var tick = await channel.Reader.ReadAsync(cancellation.Token);

        await cancellation.CancelAsync();
        await readTask;

        Assert.Equal("TEST-USD", tick.Symbol.Value);
        Assert.True(source.StreamOpenCount >= 2);
    }

    private sealed class FailingOnceTradingDataSource : ITradingDataSource
    {
        private int _streamOpenCount;

        public int StreamOpenCount => _streamOpenCount;

        public TradingDataSourceName Source => new("FailingOnceExchange");

        public async IAsyncEnumerable<MarketTick> StreamTicksAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (Interlocked.Increment(ref _streamOpenCount) == 1)
            {
                await Task.Yield();
                throw new MockExchangeDisconnectedException(Source.Value);
            }

            yield return MarketTick.Create(
                "TEST-USD",
                10m,
                2m,
                new DateTime(2026, 4, 15, 12, 0, 0, DateTimeKind.Utc),
                Source.Value);

            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
        }
    }
}
