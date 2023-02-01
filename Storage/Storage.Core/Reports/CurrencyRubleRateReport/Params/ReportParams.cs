namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;

public class ReportParams : IReportParams<string>
{
    public ReportParams()
    {
        DateFrom = DateTime.MinValue.Date;
        DateTo = DateTime.MaxValue.Date;
    }

    public ReportParams(DateTime dateFrom, DateTime dateTo)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
    }

    public string ReportKey => "CurrencyRubleRate";

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public static IList<string> IsoCharCodes => new List<string>() { "RUB" };
}
