namespace CurrencyExchange.Storage.Database.Repositories;

public interface ICurrencyExchangeRepository
{
    ICurrencyReferenceRepository CurrencyReference { get; }

    ICurrencyRateRepository CurrencyRate { get; }

    Task SaveAsync();
}
