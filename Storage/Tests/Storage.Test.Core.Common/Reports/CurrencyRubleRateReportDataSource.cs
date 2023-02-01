using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;

namespace CurrencyExchange.Storage.Test.Core.Common.Reports;

public static class CurrencyRubleRateReportDataSource
{
    public static Report Get(DateTime dateFrom, DateTime dateTo)
    {
        var report = new Report(new List<ReportRow>()
    {
        new ReportRow(new ReportValuteInfo("Валюта 1", "Valute 1", "101"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 2", "Valute 2", "102"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 3", "Valute 3", "123"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 4", "Valute 4", "321"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 5", "Valute 5", "777"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 6", "Valute 6", "128"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 7", "Valute 7", "256"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 8", "Valute 8", "512"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 9", "Valute 9", "104"), Generate10RandomReportItems(dateFrom, dateTo)),
        new ReportRow(new ReportValuteInfo("Валюта 10", "Valute 10", "105"), Generate10RandomReportItems(dateFrom, dateTo)),
    });

        return report;

        static IEnumerable<ReportItem> Generate10RandomReportItems(DateTime dateFrom, DateTime dateTo)
        {
            var nextDate = dateFrom;
            var random = new Random();
            var items = new List<ReportItem>();

            while (nextDate <= dateTo)
            {
                if (random.Next(1, 2) % 2 == 0)
                {
                    continue;
                }

                items.Add(new ReportItem(nextDate, Convert.ToDecimal(random.NextDouble() * 10)));

                nextDate = nextDate.AddDays(1);
            }

            return items;
        }
    }
}