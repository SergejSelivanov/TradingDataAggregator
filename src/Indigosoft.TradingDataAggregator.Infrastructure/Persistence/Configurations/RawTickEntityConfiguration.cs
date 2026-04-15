using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Indigosoft.TradingDataAggregator.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the raw tick persistence model.
/// </summary>
public sealed class RawTickEntityConfiguration : IEntityTypeConfiguration<RawTickEntity>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RawTickEntity> builder)
    {
        builder.ToTable("raw_ticks");
        builder.HasKey(tick => tick.Id);

        builder.Property(tick => tick.Symbol)
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(tick => tick.Source)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(tick => tick.Price)
            .HasPrecision(18, 8);

        builder.Property(tick => tick.Volume)
            .HasPrecision(18, 8);

        builder.Property(tick => tick.TimestampUtc)
            .IsRequired();

        builder.Property(tick => tick.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(tick => new { tick.Source, tick.Symbol, tick.TimestampUtc });
    }
}
