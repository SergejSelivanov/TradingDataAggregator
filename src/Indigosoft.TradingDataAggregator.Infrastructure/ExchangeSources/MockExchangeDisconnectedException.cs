namespace Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;

/// <summary>
/// Represents a simulated websocket disconnection from a mock exchange.
/// </summary>
public sealed class MockExchangeDisconnectedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MockExchangeDisconnectedException"/> class.
    /// </summary>
    public MockExchangeDisconnectedException(string source)
        : base($"Mock websocket connection was closed by {source}.")
    {
    }
}
