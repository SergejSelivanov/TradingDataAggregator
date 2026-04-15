using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Application.Options;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Application.Ingestion;

/// <summary>
/// Processes validated market ticks from the ingestion channel and persists accepted ticks.
/// </summary>
public sealed class TradingDataProcessorBackgroundService : BackgroundService
{
    private readonly Channel<MarketTick> _channel;
    private readonly ITickDeduplicationService _deduplicationService;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IngestionOptions _options;
    private readonly ILogger<TradingDataProcessorBackgroundService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TradingDataProcessorBackgroundService"/> class.
    /// </summary>
    public TradingDataProcessorBackgroundService(
        Channel<MarketTick> channel,
        ITickDeduplicationService deduplicationService,
        IServiceScopeFactory scopeFactory,
        IOptions<IngestionOptions> options,
        ILogger<TradingDataProcessorBackgroundService> logger)
    {
        _channel = channel;
        _deduplicationService = deduplicationService;
        _scopeFactory = scopeFactory;
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var batch = new List<MarketTick>(_options.MaxBatchSize);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DrainAvailableTicks(batch);

                if (batch.Count >= _options.MaxBatchSize)
                {
                    await FlushAsync(batch, cancellationToken);
                    continue;
                }

                var waitForData = _channel.Reader.WaitToReadAsync(cancellationToken).AsTask();
                var waitForFlush = Task.Delay(_options.FlushInterval, cancellationToken);
                var completedTask = await Task.WhenAny(waitForData, waitForFlush);

                if (completedTask == waitForFlush)
                {
                    await FlushIfNeededAsync(batch, cancellationToken);
                    continue;
                }

                if (!await waitForData)
                    break;
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("Market tick processor is stopping.");
        }

        if (batch.Count > 0)
        {
            using var flushTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await FlushAsync(batch, flushTimeout.Token);
        }
    }

    private void DrainAvailableTicks(ICollection<MarketTick> batch)
    {
        while (batch.Count < _options.MaxBatchSize && _channel.Reader.TryRead(out var tick))
        {
            if (!_deduplicationService.TryAccept(tick))
            {
                _logger.LogDebug(
                    "Duplicate market tick skipped: {Source} {Symbol} {TimestampUtc}",
                    tick.Source,
                    tick.Symbol,
                    tick.TimestampUtc);
                continue;
            }

            batch.Add(tick);
        }
    }

    private async Task FlushIfNeededAsync(List<MarketTick> batch, CancellationToken cancellationToken)
    {
        if (batch.Count == 0)
            return;

        await FlushAsync(batch, cancellationToken);
    }

    private async Task FlushAsync(List<MarketTick> batch, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IMarketTickRepository>();

        await repository.AddRangeAsync(batch, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Persisted {TickCount} market ticks.", batch.Count);
        batch.Clear();
    }
}
