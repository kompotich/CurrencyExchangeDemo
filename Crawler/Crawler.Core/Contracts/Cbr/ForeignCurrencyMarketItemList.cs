using System.Xml.Serialization;

namespace Crawler.Core.Contracts.Cbr;

[XmlRoot("ValCurs")]
public class ForeignCurrencyMarketItemList
{
    [XmlElement("Valute")]
    public List<ForeignCurrencyMarketItem> ForeignCurrencyMarketItems { get; set; } =
        new List<ForeignCurrencyMarketItem>();
}
