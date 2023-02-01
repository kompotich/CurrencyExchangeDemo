using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.ExchangeData.Queue;

public class CurrencyReferenceItemList : QueueItem<CurrencyReferenceItem>
{
    public CurrencyReferenceItemList(IEnumerable<CurrencyReferenceItem> values) : base(values)
    {
    }
}
