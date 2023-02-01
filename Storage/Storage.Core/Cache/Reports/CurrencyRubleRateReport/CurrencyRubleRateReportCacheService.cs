using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;

public class CurrencyRubleRateReportCacheService : ICurrencyRubleRateReportCacheService
{
    private readonly IDistributedCache _cache;

    public CurrencyRubleRateReportCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task SetAsync(Report report, ReportParams @params)
    {
        var reportKey = @params.MakeyReportKey();
        await _cache.SetAsync(reportKey, JsonSerializer.SerializeToUtf8Bytes(report));
        var paramsKey = nameof(CurrencyRubleRateReportCachedParamList);
        var cachedParams = await GetCachedParamsAsync(paramsKey);
        cachedParams ??= new CurrencyRubleRateReportCachedParamList(new List<CurrencyRubleRateReportCachedParam>()
            {
                new CurrencyRubleRateReportCachedParam(@params.DateFrom, @params.DateTo, @params.MakeyReportKey())
            });
        await _cache.SetAsync(paramsKey, JsonSerializer.SerializeToUtf8Bytes(cachedParams));
    }

    public async Task<Report?> GetAsync(ReportParams @params)
    {
        var cachedReport = await _cache.GetAsync(@params.MakeyReportKey());
        return cachedReport == null ? null : JsonSerializer.Deserialize<Report?>(cachedReport);
    }

    public async Task<CurrencyRubleRateReportCachedParamList?> GetCachedParamsAsync(string key)
    {
        var serializedCachedParams = await _cache.GetAsync(key);
        return serializedCachedParams == null
            ? null
            : JsonSerializer.Deserialize<CurrencyRubleRateReportCachedParamList>(serializedCachedParams);
    }
}
