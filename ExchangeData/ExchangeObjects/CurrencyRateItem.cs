namespace CurrencyExchange.ExchangeData.ExchangeObjects;

public class CurrencyRateItem : IExchangeObject
{
    public CurrencyRateToRuble? CurrencyRate { get; set; }

    public CurrencyRateToRuble? BaseCurrencyRate { get; set; }

    public DateTime RequestDate { get; set; }

    public decimal Value { get; set; }
}
