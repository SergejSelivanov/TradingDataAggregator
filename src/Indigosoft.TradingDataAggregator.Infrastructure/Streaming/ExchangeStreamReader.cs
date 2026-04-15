using System.Threading.Channels;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Streaming;

/// <summary>
/// Reads a market data source and reconnects when its stream fails.
/// </summary>
public sealed class ExchangeStreamReader : IExchangeStreamReader
{
    private readonly ReconnectOptions _options;
    private readonly ILogger<ExchangeStreamReader> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExchangeStreamReader"/> class.
    /// </summary>
    public ExchangeStreamReader(
        IOptions<ReconnectOptions> options,
        ILogger<ExchangeStreamReader> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ReadAsync(
        ITradingDataSource source,
        ChannelWriter<MarketTick> output,
        CancellationToken cancellationToken)
    {
        var attempt = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Market data source connected: {Source}", source.Source);

                await foreach (var tick in source.StreamTicksAsync(cancellationToken))
                {
                    attempt = 0;
                    await output.WriteAsync(tick, cancellationToken);
                }

                _logger.LogWarning("Market data source stream ended: {Source}", source.Source);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Market data source stopped: {Source}", source.Source);
                break;
            }
            catch (MockExchangeDisconnectedException exception)
            {
                _logger.LogWarning(
                    "Market data source disconnected: {Source}. {Reason}",
                    source.Source,
                    exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Market data source disconnected: {Source}", source.Source);
            }

            var delay = _options.GetDelay(++attempt);
            _logger.LogInformation(
                "Reconnecting market data source after {Delay}: {Source}",
                delay,
                source.Source);

            try
            {
                await Task.Delay(delay, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Market data source stopped: {Source}", source.Source);
                break;
            }
        }
    }
}
