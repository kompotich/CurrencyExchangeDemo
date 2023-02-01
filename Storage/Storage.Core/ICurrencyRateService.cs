using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.Storage.Core.Filters;
using CurrencyExchange.Storage.Core.Models;

namespace CurrencyExchange.Storage.Core;

public interface ICurrencyRateService
{
    Task AddAsync(CurrencyRateItem currencyRate);

    Task AddRangeAsync(IEnumerable<CurrencyRateItem> currencyRates);

    Task<IList<CurrencyRateModel>> GetAsync(CurrencyRateFilter filter);
}
