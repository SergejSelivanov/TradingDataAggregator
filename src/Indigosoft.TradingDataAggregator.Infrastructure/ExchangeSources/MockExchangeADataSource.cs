using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;
using Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;

/// <summary>
/// Generates mock websocket messages for exchange A.
/// </summary>
public sealed class MockExchangeADataSource : MockExchangeSource<ExchangeAMessage>
{
    private static readonly string[] Symbols = ["BTC-USD", "ETH-USD", "SOL-USD"];
    private readonly Random _random = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MockExchangeADataSource"/> class.
    /// </summary>
    public MockExchangeADataSource(IExchangeMessageParser<ExchangeAMessage> parser)
        : base("MockExchangeA", parser, TimeSpan.FromMilliseconds(35), 700)
    {
    }

    /// <inheritdoc />
    protected override ExchangeAMessage CreateMessage()
    {
        var symbol = Symbols[_random.Next(Symbols.Length)];
        var price = Math.Round(20_000m + (decimal)_random.NextDouble() * 3_000m, 2);
        var volume = Math.Round(0.1m + (decimal)_random.NextDouble() * 3m, 4);

        return new ExchangeAMessage(symbol, price, volume, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
    }
}
