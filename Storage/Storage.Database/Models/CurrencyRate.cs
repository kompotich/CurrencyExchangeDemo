namespace CurrencyExchange.Storage.Database.Entities.Models;

public class CurrencyRate : CurrencyExchangeEntityBase
{
    public int BaseCurrencyReferenceId { get; set; }

    public int CurrencyReferenceId { get; set; }

    public DateTime RequestDate { get; set; }

    public decimal Value { get; set; }

    public virtual CurrencyReference? CurrencyReference { get; set; }

    public virtual CurrencyReference? BaseCurrencyReference { get; set; }
}
