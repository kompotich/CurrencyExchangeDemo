namespace CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;

public class CurrencyRubleRateReportCachedParamList : CachedReportParamInfo<CurrencyRubleRateReportCachedParam>
{
    public CurrencyRubleRateReportCachedParamList()
    {
    }

    public CurrencyRubleRateReportCachedParamList(IEnumerable<CurrencyRubleRateReportCachedParam> @params) 
        : base(@params)
    {
    }
}
