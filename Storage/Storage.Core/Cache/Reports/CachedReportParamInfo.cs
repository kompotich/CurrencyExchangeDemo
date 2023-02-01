using System.Text.Json.Serialization;

namespace CurrencyExchange.Storage.Core.Cache.Reports;

public abstract class CachedReportParamInfo<T>
{
    [JsonConstructor]
    public CachedReportParamInfo()
    {
    }

    public CachedReportParamInfo(IEnumerable<T> @params)
    {
        Params = @params.ToList();
    }

    public IList<T>? Params { get; set; }
}
