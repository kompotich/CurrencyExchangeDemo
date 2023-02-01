namespace CurrencyExchange.Storage.Core.Reports;

public interface IReportParams<T>
{
    T ReportKey { get; }
}
