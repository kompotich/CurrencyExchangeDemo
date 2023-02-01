namespace CurrencyExchange.Storage.Core.Models;

public class CurrencyRateModel
{
    public string? BaseCurrencyName { get; set; }
    public string? BaseCurrencyEngName { get; set; }
    public string? BaseCurrencyIsoCharCode { get; set; }
    public string? CurrencyName { get; set; }
    public string? CurrencyEngName { get; set; }
    public string? CurrencyIsoCharCode { get; set; }
    public DateTime RequestDate { get; set; }
    public decimal Value { get; set; }
}
