using CurrencyExchange.Storage.Core.Reports;

namespace CurrencyExchange.Storage.Core.Cache.Reports;

public interface IReportCacheService<TReport, TReportKey, TReportParams>
    where TReport : IReport
    where TReportParams : IReportParams<TReportKey>
{
    Task SetAsync(TReport report, TReportParams @params);

    Task<TReport?> GetAsync(TReportParams @params);
}
