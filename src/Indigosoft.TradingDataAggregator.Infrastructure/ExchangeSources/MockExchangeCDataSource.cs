using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;
using Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;

/// <summary>
/// Generates mock websocket messages for exchange C.
/// </summary>
public sealed class MockExchangeCDataSource : MockExchangeSource<ExchangeCMessage>
{
    private static readonly (string BaseAsset, string QuoteAsset)[] Symbols =
    [
        ("BTC", "USD"),
        ("ETH", "USD"),
        ("XRP", "USD")
    ];

    private readonly Random _random = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MockExchangeCDataSource"/> class.
    /// </summary>
    public MockExchangeCDataSource(IExchangeMessageParser<ExchangeCMessage> parser)
        : base("MockExchangeC", parser, TimeSpan.FromMilliseconds(55), 1_100)
    {
    }

    /// <inheritdoc />
    protected override ExchangeCMessage CreateMessage()
    {
        var symbol = Symbols[_random.Next(Symbols.Length)];
        var price = Math.Round(500m + (decimal)_random.NextDouble() * 60_000m, 2);
        var volume = Math.Round(0.05m + (decimal)_random.NextDouble() * 5m, 4);

        return new ExchangeCMessage(symbol.BaseAsset, symbol.QuoteAsset, price, volume, DateTimeOffset.UtcNow);
    }
}
