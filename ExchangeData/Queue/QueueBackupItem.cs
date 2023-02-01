using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.ExchangeData.Queue
{
    public abstract class QueueBackupItem<T> : QueueItem<T>
        where T : IExchangeObject
    {
        public QueueBackupItem(IEnumerable<T> values) : base(values)
        {
        }

        public DateTime? LastProcessRequestDate { get; set; }
    }
}
