namespace CurrencyExchange.Storage.Core.Reports;

public interface IReportService<TReport, TReportParams, TReportKey>
    where TReport : IReport
    where TReportParams : IReportParams<TReportKey>
{
    Task<TReport> GenerateAsync(TReportParams @params);

    Task<byte[]> GenerateDocumentAsync(TReportParams @params);

    byte[] GenerateDocument(TReport report);
}
