namespace CurrencyExchange.Storage.Core.Filters;

public class CurrencyRateFilter
{
    public DateTime? RequestDateFrom { get; set; }

    public DateTime? RequestDateTo { get;set; }

    public ICollection<string>? IsoCharCodes { get; set; }
}
