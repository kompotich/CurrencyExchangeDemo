namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;

public static class ReportParamsExtensions
{
    public static string MakeyReportKey(this ReportParams @params) =>
        $"{@params.ReportKey}_{@params.DateFrom}_{@params.DateTo:dd.MM.yyyy}";
}
