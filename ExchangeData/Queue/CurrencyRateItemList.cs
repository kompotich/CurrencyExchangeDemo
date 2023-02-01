using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.ExchangeData.Queue;

public class CurrencyRateItemList : QueueBackupItem<CurrencyRateItem>
{
    public CurrencyRateItemList(IEnumerable<CurrencyRateItem> values) : base(values)
    {
    }
}
