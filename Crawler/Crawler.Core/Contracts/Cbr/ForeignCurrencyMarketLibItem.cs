using System.Xml.Serialization;

namespace Crawler.Core.Contracts.Cbr;

public class ForeignCurrencyMarketLibItem
{
    [XmlAttribute("ID")]
    public string? Id { get; set; }

    [XmlElement("Name")]
    public string? Name { get; set; }

    [XmlElement("EngName")]
    public string? EngName { get; set; }

    [XmlElement("Nominal")]
    public string? Nominal { get; set; }

    [XmlElement("ParentCode")]
    public string? ParentCode { get; set; }

    [XmlElement("ISO_Num_Code", IsNullable = true)]
    public string? IsoNumCode { get; set; }

    [XmlElement("ISO_Char_Code")]
    public string? IsoCharCode { get; set; }
}
