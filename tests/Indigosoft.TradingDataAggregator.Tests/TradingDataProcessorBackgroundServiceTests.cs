using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Application.Deduplication;
using Indigosoft.TradingDataAggregator.Application.Ingestion;
using Indigosoft.TradingDataAggregator.Application.Options;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Tests;

/// <summary>
/// Verifies application-level market tick processing.
/// </summary>
public sealed class TradingDataProcessorBackgroundServiceTests
{
    /// <summary>
    /// Ensures the processor skips duplicates and persists accepted ticks.
    /// </summary>
    [Fact]
    public async Task Processor_persists_unique_ticks_only()
    {
        var options = Options.Create(new IngestionOptions
        {
            MaxBatchSize = 2,
            FlushInterval = TimeSpan.FromMilliseconds(20)
        });
        var channel = Channel.CreateUnbounded<MarketTick>();
        var repository = new RecordingMarketTickRepository();
        using var services = new ServiceCollection()
            .AddSingleton<IMarketTickRepository>(repository)
            .BuildServiceProvider();
        var processor = new TradingDataProcessorBackgroundService(
            channel,
            new TickDeduplicationService(options),
            services.GetRequiredService<IServiceScopeFactory>(),
            options,
            NullLogger<TradingDataProcessorBackgroundService>.Instance);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        await processor.StartAsync(cancellation.Token);

        var first = CreateTick(42_000m);
        await channel.Writer.WriteAsync(first, cancellation.Token);
        await channel.Writer.WriteAsync(first, cancellation.Token);
        await channel.Writer.WriteAsync(CreateTick(42_001m), cancellation.Token);

        await WaitUntilAsync(() => repository.SavedTicks.Count == 2, cancellation.Token);
        await processor.StopAsync(cancellation.Token);

        Assert.Equal(2, repository.SavedTicks.Count);
    }

    private static MarketTick CreateTick(decimal price) =>
        MarketTick.Create(
            "BTC-USD",
            price,
            1.5m,
            new DateTime(2026, 4, 15, 12, 0, 0, DateTimeKind.Utc),
            "MockExchangeA");

    private static async Task WaitUntilAsync(Func<bool> condition, CancellationToken cancellationToken)
    {
        while (!condition())
            await Task.Delay(10, cancellationToken);
    }

    private sealed class RecordingMarketTickRepository : IMarketTickRepository
    {
        public List<MarketTick> SavedTicks { get; } = [];
        private readonly List<MarketTick> _pendingTicks = [];

        public Task AddRangeAsync(IReadOnlyCollection<MarketTick> ticks, CancellationToken cancellationToken)
        {
            _pendingTicks.AddRange(ticks);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            SavedTicks.AddRange(_pendingTicks);
            _pendingTicks.Clear();
            return Task.CompletedTask;
        }
    }
}
