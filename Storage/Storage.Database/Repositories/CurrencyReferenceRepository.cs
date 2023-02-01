using CurrencyExchange.Storage.Database.Entities;
using CurrencyExchange.Storage.Database.Entities.Models;

namespace CurrencyExchange.Storage.Database.Repositories;

public class CurrencyReferenceRepository : 
    CurrencyExchangeRepositoryBase<CurrencyReference>, 
    ICurrencyReferenceRepository
{
    public CurrencyReferenceRepository(CurrencyExchangeContext currencyExchangeContext) 
        : base(currencyExchangeContext)
    {
    }
}
