using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.ExchangeData.Queue;

public abstract class QueueItem<T>
    where T : IExchangeObject
{
    public QueueItem(IEnumerable<T> values)
    {
        Id = Guid.NewGuid();
        Values = values?.ToList();
    }

    public Guid Id { get; init; }

    public IList<T>? Values { get; init; }
}
