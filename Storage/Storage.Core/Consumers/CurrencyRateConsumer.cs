using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.ExchangeData.Queue;
using CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;
using MassTransit;

namespace CurrencyExchange.Storage.Core.Consumers;

public class CurrencyRateConsumer : IConsumer<CurrencyRateItemList>
{
    private readonly ICurrencyRateService _currencyRateService;
    private readonly ICurrencyRubleRateReportService _currencyRubleRateReportService;
    private readonly ICurrencyRubleRateReportCacheService _currencyRubleRateReportCacheService;

    public CurrencyRateConsumer(
        ICurrencyRateService currencyRateService,
        ICurrencyRubleRateReportService currencyRubleRateReportService,
        ICurrencyRubleRateReportCacheService currencyRubleRateReportCacheService)
    {
        _currencyRateService = currencyRateService;
        _currencyRubleRateReportService = currencyRubleRateReportService;
        _currencyRubleRateReportCacheService = currencyRubleRateReportCacheService;
    }

    public async Task Consume(ConsumeContext<CurrencyRateItemList> context)
    {
        if (context.Message.Values == null)
        {
            return;
        }

        await _currencyRateService.AddRangeAsync(context.Message.Values);

        foreach(var currencyRate in context.Message.Values)
        {
            await UpdateCurrencyRubleRateCachedReports(currencyRate);
        }
    }

    private async Task UpdateCurrencyRubleRateCachedReports(CurrencyRateItem currencyRate)
    {
        var obsoleteCachedReportDataKeys = (await _currencyRubleRateReportCacheService
            .GetCachedParamsAsync(nameof(CurrencyRubleRateReportCachedParamList)))?.Params
            ?.Where(x => x.DateFrom <= currencyRate.RequestDate && x.DateTo <= currencyRate.RequestDate)
            .ToList();

        if (!obsoleteCachedReportDataKeys.Any())
        {
            return;
        }

        foreach (var obsoleteCachedReportDataKey in obsoleteCachedReportDataKeys)
        {
            var reportParams = new ReportParams(obsoleteCachedReportDataKey.DateFrom, obsoleteCachedReportDataKey.DateTo);
            var report = await _currencyRubleRateReportService.GenerateAsync(reportParams);
            await _currencyRubleRateReportCacheService.SetAsync(report, reportParams);
        }
    }
}
