using System.Globalization;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;
using Indigosoft.TradingDataAggregator.Infrastructure.Parsing;

namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;

/// <summary>
/// Generates mock websocket messages for exchange B.
/// </summary>
public sealed class MockExchangeBDataSource : MockExchangeSource<ExchangeBMessage>
{
    private static readonly string[] Symbols = ["BTCUSD", "ETHUSD", "ADAUSD"];
    private readonly Random _random = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MockExchangeBDataSource"/> class.
    /// </summary>
    public MockExchangeBDataSource(IExchangeMessageParser<ExchangeBMessage> parser)
        : base("MockExchangeB", parser, TimeSpan.FromMilliseconds(45), 900)
    {
    }

    /// <inheritdoc />
    protected override ExchangeBMessage CreateMessage()
    {
        var symbol = Symbols[_random.Next(Symbols.Length)];
        var price = Math.Round(1_500m + (decimal)_random.NextDouble() * 2_500m, 2);
        var volume = Math.Round(0.5m + (decimal)_random.NextDouble() * 8m, 4);

        return new ExchangeBMessage(
            symbol,
            price.ToString(CultureInfo.InvariantCulture),
            volume.ToString(CultureInfo.InvariantCulture),
            DateTime.UtcNow);
    }
}
