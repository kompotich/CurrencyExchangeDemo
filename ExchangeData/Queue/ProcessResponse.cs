namespace CurrencyExchange.ExchangeData.Queue
{
    public class ProcessResponse
    {
        public Guid RequestId { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public DateTime? LastProcessRequestDate { get; set; }
    }
}
