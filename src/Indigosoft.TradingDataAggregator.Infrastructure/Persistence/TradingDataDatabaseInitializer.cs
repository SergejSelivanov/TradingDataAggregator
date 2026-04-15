using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence;

/// <summary>
/// Initializes the market data database used by the host.
/// </summary>
public static class TradingDataDatabaseInitializer
{
    /// <summary>
    /// Ensures that the SQLite database schema exists.
    /// </summary>
    public static async Task EnsureDatabaseCreatedAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TradingDataDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
    }
}
