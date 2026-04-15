using Microsoft.EntityFrameworkCore;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence;

/// <summary>
/// Provides EF Core access to the local market data SQLite database.
/// </summary>
public sealed class TradingDataDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TradingDataDbContext"/> class.
    /// </summary>
    public TradingDataDbContext(DbContextOptions<TradingDataDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the persisted raw ticks.
    /// </summary>
    public DbSet<RawTickEntity> RawTicks => Set<RawTickEntity>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TradingDataDbContext).Assembly);
}
