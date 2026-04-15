namespace Indigosoft.TradingDataAggregator.Infrastructure.Streaming;

/// <summary>
/// Defines reconnect retry delays for exchange stream readers.
/// </summary>
public sealed class ReconnectOptions
{
    /// <summary>
    /// Gets or sets the initial reconnect delay.
    /// </summary>
    public TimeSpan InitialDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Gets or sets the maximum reconnect delay.
    /// </summary>
    public TimeSpan MaxDelay { get; set; } = TimeSpan.FromSeconds(15);

    /// <summary>
    /// Gets the reconnect delay for the specified retry attempt.
    /// </summary>
    public TimeSpan GetDelay(int attempt) =>
        TimeSpan.FromMilliseconds(Math.Min(
            InitialDelay.TotalMilliseconds * Math.Pow(2, Math.Max(0, attempt - 1)),
            MaxDelay.TotalMilliseconds));
}
