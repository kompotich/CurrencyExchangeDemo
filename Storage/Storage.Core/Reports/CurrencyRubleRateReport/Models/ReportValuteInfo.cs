using System.Text.Json.Serialization;

namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;

public class ReportValuteInfo
{
    [JsonConstructor]
    public ReportValuteInfo()
    {
    }

    public ReportValuteInfo(string name, string engName, string isoCharCode)
    {
        Name = name;
        EngName = engName;
        IsoCharCode = isoCharCode;
    }

    public string? Name { get; set; }

    public string? EngName { get; set; }

    public string? IsoCharCode { get; set; }
}
