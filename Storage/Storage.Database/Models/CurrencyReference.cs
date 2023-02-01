namespace CurrencyExchange.Storage.Database.Entities.Models;

public class CurrencyReference : CurrencyExchangeEntityBase
{
    public string? Name { get; set; }

    public string? EngName { get; set; }

    public string? ParentCode { get; set; }

    public string? IsoCharCode { get; set; }

    public virtual ICollection<CurrencyRate>? CurrencyRates { get; set; }
    
    public virtual ICollection<CurrencyRate>? BaseCurrencyRates { get; set; }
}
