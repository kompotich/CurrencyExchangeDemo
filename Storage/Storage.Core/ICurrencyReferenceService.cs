using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.Storage.Core.Models;

namespace CurrencyExchange.Storage.Core;

public interface ICurrencyReferenceService
{
    Task AddAsync(CurrencyReferenceItem currencyRubleInfo);

    Task AddRangeAsync(IEnumerable<CurrencyReferenceItem> currencyRubleInfos);

    Task<IList<CurrencyReferenceModel>> GetAllAsync();

    Task UpdateAsync(CurrencyReferenceItem currencyRubleInfo);

    Task UpdateRangeAsync(IEnumerable<CurrencyReferenceItem> currencyRubleInfos);
}
