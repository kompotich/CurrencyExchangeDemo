using CurrencyExchange.Crawler.Database.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Crawler.Database.Entities;

public class CurrencyExchangeContext : DbContext
{
    public CurrencyExchangeContext(DbContextOptions options) : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Setting> Settings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setting>()
            .Property(x => x.Type)
            .HasConversion(
                x => x.ToString(), 
                x => (SettingValueType)Enum.Parse(typeof(SettingValueType), x));

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
}
