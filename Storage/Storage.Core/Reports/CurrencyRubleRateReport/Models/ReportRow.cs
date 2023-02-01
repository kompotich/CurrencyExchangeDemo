using System.Text.Json.Serialization;

namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;

public class ReportRow
{
    [JsonConstructor]
    public ReportRow()
    {
    }

    public ReportRow(ReportValuteInfo valuteInfo, IEnumerable<ReportItem> items)
    {
        ValuteInfo = valuteInfo;
        Items = items.ToList();
    }

    public ReportValuteInfo? ValuteInfo { get; set; }

    public IList<ReportItem>? Items { get; set; }

    public decimal AvgValue => Items?.Average(x => x.Value) ?? 0;

    public string Emoji => AvgValue == 0
        ? "-"
        : AvgValue < 1
            ? "😀"
            : AvgValue >= 1 && AvgValue < 10
                ? "😐"
                : "😕";
}
