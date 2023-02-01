namespace CurrencyExchange.Crawler.Core.Models;

public class CurrencyRateModel
{
    public string? Name { get; set; }
    public string? EngName { get; set; }
    public string? IsoCharCode { get; set; }
    public int Nominal { get; set; }
    public DateTime RequestDate { get; set; }
    public decimal Value { get; set; }
}
