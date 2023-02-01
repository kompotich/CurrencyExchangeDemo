namespace CurrencyExchange.ExchangeData.ExchangeObjects;

public class CurrencyReferenceItem : IExchangeObject
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? EngName { get; set; }

    public int Nominal { get; set; }

    public string? ParentCode { get; set; }

    public int IsoNumCode { get; set; }

    public string? IsoCharCode { get; set; }
}