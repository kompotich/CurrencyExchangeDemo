using CurrencyExchange.Storage.Database.Entities;

namespace CurrencyExchange.Storage.Database.Repositories;

public class CurrencyRateRepository :
    CurrencyExchangeRepositoryBase<Entities.Models.CurrencyRate>,
    ICurrencyRateRepository
{
    public CurrencyRateRepository(CurrencyExchangeContext currencyExchangeContext) : base(currencyExchangeContext)
    {
    }
}
