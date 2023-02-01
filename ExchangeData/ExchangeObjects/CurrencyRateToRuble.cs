namespace CurrencyExchange.ExchangeData.ExchangeObjects;

public class CurrencyRateToRuble : IExchangeObject
{
    public CurrencyReferenceItem? CurrencyReferenceItem { get; set; }

    public DateTime RequestDate { get; set; }

    public decimal Value { get; set; }

}