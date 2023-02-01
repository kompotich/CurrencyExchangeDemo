using System.Text.Json.Serialization;

namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;

public class ReportItem
{
    [JsonConstructor]
    public ReportItem()
    {
    }

    public ReportItem(DateTime date, decimal value)
    {
        Date = date;
        Value = value;
    }

    public DateTime Date { get; set; }

    public decimal Value { get; set; }
}
