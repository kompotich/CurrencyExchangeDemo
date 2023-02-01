using CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;

namespace CurrencyExchange.Storage.Core.Cache;

public class CacheHostedService : BackgroundService
{
    private const int _clearCachedInterval = 1000 * 60 * 60 * 24 * 3;

    private readonly IDistributedCache _cache;
    private readonly ICurrencyRubleRateReportCacheService _currencyRubleRateReportService;

    public CacheHostedService(
        IDistributedCache cache,
        ICurrencyRubleRateReportCacheService currencyRubleRateReportService)
    {
        _cache = cache;
        _currencyRubleRateReportService = currencyRubleRateReportService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(_clearCachedInterval, stoppingToken);

            await ClearCurrencyRubleRateReportCache(stoppingToken);
        }
    }

    private async Task ClearCurrencyRubleRateReportCache(CancellationToken cancellationToken)
    {
        var cachedParamsKey = nameof(CurrencyRubleRateReportCachedParamList);
        await _cache.RemoveAsync(cachedParamsKey, cancellationToken);
        
        var cachedReportParams = (await _currencyRubleRateReportService.GetCachedParamsAsync(cachedParamsKey))?.Params;
        
        if (cachedReportParams == null)
        {
            return;
        }

        foreach(var cachedReportParam in cachedReportParams)
        {
            await _cache.RemoveAsync(cachedReportParam.Key, cancellationToken);
        }
    }
}
