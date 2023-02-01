using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;

namespace CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;

public interface ICurrencyRubleRateReportCacheService : IReportCacheService<Report, string, ReportParams>
{
    Task<CurrencyRubleRateReportCachedParamList?> GetCachedParamsAsync(string key);
}
