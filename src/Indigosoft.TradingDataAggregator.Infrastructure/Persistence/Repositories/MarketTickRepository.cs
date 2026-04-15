using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Domain.TradingData;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Repositories;

/// <summary>
/// Persists market ticks using EF Core.
/// </summary>
public sealed class MarketTickRepository : IMarketTickRepository
{
    private readonly TradingDataDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="MarketTickRepository"/> class.
    /// </summary>
    public MarketTickRepository(TradingDataDbContext context) =>
        _context = context;

    /// <inheritdoc />
    public async Task AddRangeAsync(IReadOnlyCollection<MarketTick> ticks, CancellationToken cancellationToken)
    {
        if (ticks.Count == 0)
            return;

        var storedAtUtc = DateTime.UtcNow;
        var entities = ticks.Select(tick => new RawTickEntity
        {
            Symbol = tick.Symbol.Value,
            Price = tick.Price.Value,
            Volume = tick.Volume.Value,
            TimestampUtc = tick.TimestampUtc,
            Source = tick.Source.Value,
            CreatedAtUtc = storedAtUtc
        });

        await _context.RawTicks.AddRangeAsync(entities, cancellationToken);
    }

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken) =>
        _context.SaveChangesAsync(cancellationToken);
}
