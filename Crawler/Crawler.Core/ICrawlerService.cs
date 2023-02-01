using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.Crawler.Core;

public interface ICrawlerService
{
    Task<IList<CurrencyReferenceItem>> GetCurrencyReferenceAsync();

    Task<IList<CurrencyRateToRuble>> GetCurrencyRatesToRubleAsync(DateTime date);
}
