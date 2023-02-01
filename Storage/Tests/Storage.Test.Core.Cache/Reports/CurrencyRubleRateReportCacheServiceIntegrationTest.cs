using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;
using CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Test.Core.Common.Reports;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.StackExchangeRedis;

namespace CurrencyExchange.Storage.Test.Integration.Core.Cache.Reports;

[TestClass]
public class CurrencyRubleRateReportCacheServiceIntegrationTest
{
    private readonly ICurrencyRubleRateReportCacheService _currencyRubleRateReportCacheService;

    public CurrencyRubleRateReportCacheServiceIntegrationTest()
    {

        _currencyRubleRateReportCacheService = new CurrencyRubleRateReportCacheService(
            new RedisCache(Options.Create(new RedisCacheOptions()
            {
                Configuration = "localhost",
                InstanceName = "cache_service_test"
            })));
    }

    [TestMethod]
    public void SetReport_GetCachedReport_NotNull()
    {
        // arrange
        var dateTo = DateTime.Now.Date;
        var dateFrom = dateTo.AddDays(-10);
        var report = CurrencyRubleRateReportDataSource.Get(dateFrom, dateTo);
        var @params = new ReportParams(dateFrom, dateTo);

        // act
        _currencyRubleRateReportCacheService.SetAsync(report, @params).Wait();
        var cachedReport = _currencyRubleRateReportCacheService.GetAsync(@params).Result;

        // assert
        Assert.IsNotNull(cachedReport);
    }

    [TestMethod]
    public void SetReport_GetCachedReport_ValuesAreEqual()
    {
        // arrange
        var dateTo = DateTime.Now.Date;
        var dateFrom = dateTo.AddDays(-10);
        var report = CurrencyRubleRateReportDataSource.Get(dateFrom, dateTo);
        var @params = new ReportParams(dateFrom, dateTo);

        // act
        _currencyRubleRateReportCacheService.SetAsync(report, @params).Wait();
        var cachedReport = _currencyRubleRateReportCacheService.GetAsync(@params).Result;

        // assert
        Assert.IsTrue(cachedReport?.Rows.Any());
        Assert.AreEqual(report.Rows.Count, cachedReport?.Rows.Count);

        for (var i = 0; i < report.Rows.Count; i++)
        {
            Assert.AreEqual(report.Rows[i].ValuteInfo.Name, cachedReport?.Rows[i].ValuteInfo.Name);
            Assert.AreEqual(report.Rows[i].ValuteInfo.EngName, cachedReport?.Rows[i].ValuteInfo.EngName);
            Assert.AreEqual(report.Rows[i].ValuteInfo.IsoCharCode, cachedReport?.Rows[i].ValuteInfo.IsoCharCode);
            Assert.AreEqual(report.Rows[i].AvgValue, cachedReport?.Rows[i].AvgValue);
            Assert.AreEqual(report.Rows[i].Emoji, cachedReport?.Rows[i].Emoji);
            Assert.AreEqual(report.Rows[i].Items.Count, cachedReport?.Rows[i].Items.Count);

            for (int j = 0; i < report.Rows.Count; i++)
            {
                Assert.AreEqual(report.Rows[i].Items[j].Date, report.Rows[i].Items[j].Date);
                Assert.AreEqual(report.Rows[i].Items[j].Value, report.Rows[i].Items[j].Value);
            }
        }
    }
}