using System.Text.Json.Serialization;

namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;

public class Report : IReport
{
    public const string PageName = "Курс_валют";
    public const string ValuteNameColumnValue = "Наименование валюты";
    public const string RowAvgValue = "Среднее значение";

    [JsonConstructor]
    public Report()
    {
    }

    public Report(IEnumerable<ReportRow> rows)
    {
        Rows = rows.ToList();
    }

    public IList<ReportRow>? Rows { get; set; }

    public static string GetMainHatValue(DateTime dateFrom, DateTime dateTo) =>
        $"Курсы валют относительно рубля с {dateFrom:dd.MM.yyyy} по {dateTo:dd.MM.yyyy}";
}
