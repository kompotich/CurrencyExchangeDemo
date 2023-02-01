using System.Text.Json.Serialization;

namespace CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;

public class CurrencyRubleRateReportCachedParam
{
    [JsonConstructor]
    public CurrencyRubleRateReportCachedParam()
    {
    }

    public CurrencyRubleRateReportCachedParam(DateTime dateFrom, DateTime dateTo, string key)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
        Key = key;
    }

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public string? Key { get; set; }
}
