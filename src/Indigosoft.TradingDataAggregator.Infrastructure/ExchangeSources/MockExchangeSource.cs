using System.Runtime.CompilerServices;
using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Domain.TradingData;
using Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;

/// <summary>
/// Provides common behavior for mock websocket market data sources.
/// </summary>
public abstract class MockExchangeSource<TMessage> : ITradingDataSource
{
    private readonly IExchangeMessageParser<TMessage> _parser;
    private readonly TimeSpan _tickInterval;
    private readonly int _disconnectEveryTicks;

    /// <summary>
    /// Initializes a new instance of the <see cref="MockExchangeSource{TMessage}"/> class.
    /// </summary>
    protected MockExchangeSource(
        string source,
        IExchangeMessageParser<TMessage> parser,
        TimeSpan tickInterval,
        int disconnectEveryTicks)
    {
        Source = new TradingDataSourceName(source);
        _parser = parser;
        _tickInterval = tickInterval;
        _disconnectEveryTicks = disconnectEveryTicks;
    }

    /// <inheritdoc />
    public TradingDataSourceName Source { get; }

    /// <inheritdoc />
    public async IAsyncEnumerable<MarketTick> StreamTicksAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        for (var tickNumber = 1; ; tickNumber++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_disconnectEveryTicks > 0 && tickNumber % _disconnectEveryTicks == 0)
                throw new MockExchangeDisconnectedException(Source.Value);

            await Task.Delay(_tickInterval, cancellationToken);
            yield return _parser.Parse(CreateMessage());
        }
    }

    /// <summary>
    /// Creates a raw source-specific mock message.
    /// </summary>
    protected abstract TMessage CreateMessage();
}
