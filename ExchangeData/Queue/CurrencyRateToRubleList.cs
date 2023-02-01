using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.ExchangeData.Queue;

public class CurrencyRateToRubleList : QueueBackupItem<CurrencyRateToRuble>
{
    public CurrencyRateToRubleList(IEnumerable<CurrencyRateToRuble> values) : base(values)
    {
    }
}
