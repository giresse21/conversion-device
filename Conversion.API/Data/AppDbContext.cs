using Microsoft.EntityFrameworkCore;
using Conversion.API.Models;

namespace Conversion.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ConversionResult> ConversionResults { get; set; } = null!;
    public DbSet<Currency> Currencies { get; set; } = null!;
    public DbSet<CurrencyRate> CurrencyRates { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ConversionResult a deux propriétés (CurrencyFrom, CurrencyTo) de type Currency.
        // Ces propriétés sont sur ConversionResult, pas sur Currency. Chacune référence une ligne de la table Currency.
        modelBuilder.Entity<ConversionResult>(
            entity =>
            {
                entity.HasOne(e => e.CurrencyFrom)  // e = ConversionResult, e.CurrencyFrom = une Currency
                    .WithMany()
                    .HasForeignKey(e => e.CurrencyFromId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CurrencyTo)    // e = ConversionResult, e.CurrencyTo = une Currency
                    .WithMany()
                    .HasForeignKey(e => e.CurrencyToId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        );

        // Idem : CurrencyRate a CurrencyFrom et CurrencyTo (propriétés sur CurrencyRate, type Currency).
        modelBuilder.Entity<CurrencyRate>(
            entity =>
            {
                entity.HasOne(e => e.CurrencyFrom)  // e = CurrencyRate
                    .WithMany()
                    .HasForeignKey(e => e.CurrencyFromId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CurrencyTo)    // e = CurrencyRate
                    .WithMany()
                    .HasForeignKey(e => e.CurrencyToId)
                    .OnDelete(DeleteBehavior.Restrict);
            }
        );
    }
}
