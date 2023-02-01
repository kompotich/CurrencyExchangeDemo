using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;
using Microsoft.AspNetCore.Mvc;

namespace Storage.Main.Controllers;

[Route("Storage/Report")]
public class ReportController : ControllerBase
{
    private readonly ICurrencyRubleRateReportService _currencyRubleRateReportService;

    public ReportController(ICurrencyRubleRateReportService currencyRubleRateReportService)
    {
        _currencyRubleRateReportService = currencyRubleRateReportService;
    }

    [Route("CurrencyRubleRate/{dateTo}")]
    [HttpGet()]
    public async Task<FileContentResult> GenerateCurrencyRubleRateReport(DateTime dateFrom, DateTime dateTo) =>
        File(await _currencyRubleRateReportService.GenerateDocumentAsync(new ReportParams(dateFrom, dateTo)),
            "application/vnd.ms-excel",
            $"Курсы_валют_относительно_рубля_({DateTime.Now:ddMMyyyyHHmmss}).xlsx");
}
