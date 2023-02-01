using System.Xml.Serialization;

namespace Crawler.Core.Contracts.Cbr;

public class ForeignCurrencyMarketItem
{
    [XmlAttribute("ID")]
    public string? Id { get; set; }

    [XmlElement("NumCode")]
    public string? NumCode { get; set; }

    [XmlElement("CharCode")]
    public string? CharCode { get; set; }

    [XmlElement("Nominal")]
    public string? Nominal { get; set; }

    [XmlElement("Name")]
    public string? Name { get; set; }

    [XmlElement("Value")]
    public string? Value { get; set; }
}
