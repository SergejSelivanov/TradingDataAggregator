using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Indigosoft.TradingDataAggregator.Application.Ingestion;

/// <summary>
/// Starts all configured market data sources and writes their ticks into the ingestion channel.
/// </summary>
public sealed class TradingDataIngestionBackgroundService : BackgroundService
{
    private readonly IReadOnlyCollection<ITradingDataSource> _sources;
    private readonly IExchangeStreamReader _streamReader;
    private readonly Channel<MarketTick> _channel;
    private readonly ILogger<TradingDataIngestionBackgroundService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TradingDataIngestionBackgroundService"/> class.
    /// </summary>
    public TradingDataIngestionBackgroundService(
        IEnumerable<ITradingDataSource> sources,
        IExchangeStreamReader streamReader,
        Channel<MarketTick> channel,
        ILogger<TradingDataIngestionBackgroundService> logger)
    {
        _sources = sources.ToArray();
        _streamReader = streamReader;
        _channel = channel;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting {SourceCount} market data sources.", _sources.Count);

        var tasks = _sources
            .Select(source => _streamReader.ReadAsync(source, _channel.Writer, cancellationToken))
            .ToArray();

        await Task.WhenAll(tasks);
    }
}
