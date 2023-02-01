using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;

namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport;

public interface ICurrencyRubleRateReportService : IReportService<Report, ReportParams, string>
{
}
