using System.Xml.Serialization;

namespace Crawler.Core.Contracts.Cbr;

[XmlRoot("Valuta")]
public class ForeignCurrencyMarketLibItemList
{
    [XmlElement("Item")]
    public List<ForeignCurrencyMarketLibItem> ForeignCurrencyMarketLibItems { get; set; } =
        new List<ForeignCurrencyMarketLibItem>();
}
