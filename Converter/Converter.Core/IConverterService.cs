using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.ExchangeData.Queue;

namespace CurrencyExchange.Converter.Core;

public interface IConverterService
{
    IList<CurrencyRateItem> CalculateCurrencyRates(IEnumerable<CurrencyRateToRuble> rates);
}
