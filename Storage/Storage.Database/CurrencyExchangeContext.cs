using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Storage.Database.Entities;

public class CurrencyExchangeContext : DbContext
{
    public CurrencyExchangeContext(DbContextOptions options) : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Models.CurrencyReference> CurrencyReferences { get; set; }

    public DbSet<Models.CurrencyRate> CurrencyRates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        BuildCurrencyReferenceModel(modelBuilder);
        BuildCurrencyRateModel(modelBuilder);
    }

    private static void BuildCurrencyReferenceModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.CurrencyReference>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Models.CurrencyReference>()
            .Property(x => x.Name)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyReference>()
            .Property(x => x.EngName)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyReference>()
            .Property(x => x.ParentCode)
            .IsRequired()
            .HasColumnName("RId");

        modelBuilder.Entity<Models.CurrencyReference>()
            .HasMany(x => x.CurrencyRates)
            .WithOne(x => x.CurrencyReference)
            .HasForeignKey(x => x.CurrencyReferenceId)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyReference>()
            .HasMany(x => x.BaseCurrencyRates)
            .WithOne(x => x.BaseCurrencyReference)
            .HasForeignKey(x => x.BaseCurrencyReferenceId)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyReference>()
            .ToTable("CurrencyReference");
    }

    private static void BuildCurrencyRateModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.CurrencyRate>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<Models.CurrencyRate>()
            .Property(x => x.BaseCurrencyReferenceId)
            .IsRequired()
            .HasColumnName("BaseCurrencyId");

        modelBuilder.Entity<Models.CurrencyRate>()
            .Property(x => x.CurrencyReferenceId)
            .IsRequired()
            .HasColumnName("CurrencyId");

        modelBuilder.Entity<Models.CurrencyRate>()
            .Property(x => x.RequestDate)
            .IsRequired()
            .HasColumnName("Date");

        modelBuilder.Entity<Models.CurrencyRate>()
            .Property(x => x.Value)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyRate>()
            .HasOne(x => x.CurrencyReference)
            .WithMany(x => x.CurrencyRates)
            .HasForeignKey(x => x.CurrencyReferenceId)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyRate>()
            .HasOne(x => x.BaseCurrencyReference)
            .WithMany(x => x.BaseCurrencyRates)
            .HasForeignKey(x => x.BaseCurrencyReferenceId)
            .IsRequired();

        modelBuilder.Entity<Models.CurrencyRate>()
            .ToTable("CurrencyRates");
    }
}
