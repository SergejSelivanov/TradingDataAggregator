using Indigosoft.TradingDataAggregator.Application.Abstractions;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeMessages;
using Indigosoft.TradingDataAggregator.Infrastructure.ExchangeSources;
using Indigosoft.TradingDataAggregator.Infrastructure.Parsing;
using Indigosoft.TradingDataAggregator.Infrastructure.Persistence;
using Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Repositories;
using Indigosoft.TradingDataAggregator.Infrastructure.Streaming;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Extensions;

/// <summary>
/// Registers Trading Data Aggregator infrastructure services.
/// </summary>
public static class TradingDataAggregatorInfrastructureExtensions
{
    /// <summary>
    /// Adds mock exchange sources, reconnect handling, and SQLite persistence.
    /// </summary>
    public static IServiceCollection AddTradingDataAggregatorInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var sqliteConnectionString = configuration.GetConnectionString("TradingData")
            ?? "Data Source=./data/market-data.db";

        EnsureSqliteDirectoryExists(sqliteConnectionString);

        services.AddDbContext<TradingDataDbContext>(options =>
            options.UseSqlite(sqliteConnectionString));

        services.AddSingleton<IExchangeMessageParser<ExchangeAMessage>, ExchangeAMessageParser>();
        services.AddSingleton<IExchangeMessageParser<ExchangeBMessage>, ExchangeBMessageParser>();
        services.AddSingleton<IExchangeMessageParser<ExchangeCMessage>, ExchangeCMessageParser>();

        services.AddSingleton<ITradingDataSource, MockExchangeADataSource>();
        services.AddSingleton<ITradingDataSource, MockExchangeBDataSource>();
        services.AddSingleton<ITradingDataSource, MockExchangeCDataSource>();

        services.AddSingleton<IExchangeStreamReader, ExchangeStreamReader>();
        services.AddScoped<IMarketTickRepository, MarketTickRepository>();

        return services;
    }

    private static void EnsureSqliteDirectoryExists(string connectionString)
    {
        var builder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(connectionString);
        var dataSource = builder.DataSource;

        if (string.IsNullOrWhiteSpace(dataSource))
            return;

        var directory = Path.GetDirectoryName(Path.GetFullPath(dataSource));
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);
    }
}
