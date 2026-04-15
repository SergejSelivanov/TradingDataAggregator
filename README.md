# Trading Data Aggregator

.NET service that reads mock exchange streams, normalizes market ticks, removes recent duplicates, batches data, and saves it to SQLite.

## Run

```bash
dotnet run --project src/Indigosoft.TradingDataAggregator.Host
```

SQLite database is created automatically at:

```text
./data/market-data.db
```

Stop the service with `Ctrl+C`.

## Test

```bash
dotnet test
```

## Configuration

Defaults are in:

```text
src/Indigosoft.TradingDataAggregator.Host/appsettings.json
```

Main settings:

- `ConnectionStrings:TradingData` - SQLite connection string
- `Ingestion:MaxBatchSize` - batch size before saving
- `Ingestion:FlushInterval` - max wait before saving a non-empty batch
- `Ingestion:DeduplicationRetentionPeriod` - duplicate detection window
- `Reconnect:InitialDelay` / `Reconnect:MaxDelay` - reconnect delays
