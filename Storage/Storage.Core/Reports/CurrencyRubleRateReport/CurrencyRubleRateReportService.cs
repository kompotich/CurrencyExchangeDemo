using ClosedXML.Excel;
using CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Filters;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Params;

namespace CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport;

public class CurrencyRubleRateReportService : ICurrencyRubleRateReportService
{
    private readonly ICurrencyRateService _currencyRateService;
    private readonly ICurrencyRubleRateReportCacheService _currencyRubleRateReportCacheService;

    public CurrencyRubleRateReportService(
        ICurrencyRateService currencyRateService,
        ICurrencyRubleRateReportCacheService currencyRubleRateReportCacheService)
    {
        _currencyRateService = currencyRateService;
        _currencyRubleRateReportCacheService = currencyRubleRateReportCacheService;
    }

    public async Task<Report> GenerateAsync(ReportParams @params)
    {
        var cachedReport = await _currencyRubleRateReportCacheService.GetAsync(@params);

        if (cachedReport != null)
        {
            return cachedReport;
        }

        var currencyRateModels = await _currencyRateService.GetAsync(new CurrencyRateFilter()
        {
            RequestDateFrom = @params.DateFrom,
            RequestDateTo = @params.DateTo,
            IsoCharCodes = ReportParams.IsoCharCodes
        });

        if (!currencyRateModels.Any())
        {
            return new Report(Enumerable.Empty<ReportRow>().ToList());
        }

        var rows = currencyRateModels.GroupBy(x => new
        {
            x.BaseCurrencyIsoCharCode,
            x.BaseCurrencyName,
            x.BaseCurrencyEngName
        }).Select(x => new ReportRow(
            new ReportValuteInfo(
                x.Key.BaseCurrencyName,
                x.Key.BaseCurrencyEngName,
                x.Key.BaseCurrencyIsoCharCode),
            x.Select(y => new ReportItem(y.RequestDate, y.Value))));

        var report = new Report(rows);

        await _currencyRubleRateReportCacheService.SetAsync(report, @params);

        return report;
    }

    public async Task<byte[]> GenerateDocumentAsync(ReportParams @params) =>
        GenerateDocument(await GenerateAsync(@params));

    public byte[] GenerateDocument(Report report) => ConvertToXlsx(report);

    #region convert to xlsx document

    private static byte[] ConvertToXlsx(Report report)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet(Report.PageName);

        if (!report.Rows.Any())
        {
            return ConvertWorkbookToByteArray(workbook);
        }

        var dateRange = report.Rows.SelectMany(x => x.Items).Select(x => x.Date).Distinct().ToList();

        FillHat(worksheet, dateRange);
        FillContent(worksheet, report.Rows, dateRange);

        worksheet.Columns().AdjustToContents();

        return ConvertWorkbookToByteArray(workbook);
    }

    private static void FillHat(IXLWorksheet worksheet, IEnumerable<DateTime> dateRange)
    {
        var currentDate = dateRange.Min();
        var dateTo = dateRange.Max();

        worksheet.Cell(1, 1).Value = Report.GetMainHatValue(currentDate, dateTo);
        var mainHatRange = worksheet.Range(1, 1, 1, dateRange.Count() + 3);
        mainHatRange.Merge();
        mainHatRange.Style.Font.Bold = true;
        mainHatRange.Style.Fill.BackgroundColor = XLColor.Orange;

        var valuteNameHatCell = worksheet.Cell(2, 1);
        valuteNameHatCell.Value = Report.ValuteNameColumnValue;
        valuteNameHatCell.Style.Font.Bold = true;

        int columnIndex = 2;

        while (currentDate <= dateTo)
        {
            var dateValueCell = worksheet.Cell(2, columnIndex++);
            dateValueCell.Value = currentDate.ToString("dd.MM.yyyy");
            dateValueCell.Style.Font.Bold = true;
            dateValueCell.Style.Fill.BackgroundColor = XLColor.Yellow;

            currentDate = currentDate.AddDays(1);
        }

        var avgValueCell = worksheet.Cell(2, ++columnIndex);
        avgValueCell.Value = Report.RowAvgValue;
        avgValueCell.Style.Font.Bold = true;
        avgValueCell.Style.Fill.BackgroundColor = XLColor.Yellow;
    }

    private static void FillContent(IXLWorksheet worksheet, IList<ReportRow> rows, IEnumerable<DateTime> dateRange)
    {
        for (var i = 0; i < rows.Count; i++)
        {
            var rowIndex = i + 3;
            var valuteNameCell = worksheet.Cell(rowIndex, 1);
            valuteNameCell.Value = $"{rows[i].ValuteInfo.Name} ({rows[i].ValuteInfo.IsoCharCode})";
            valuteNameCell.Style.Fill.BackgroundColor = XLColor.Green;
            
            FillRowContent(worksheet, rows[i], dateRange, rowIndex);
        }
    }

    private static void FillRowContent(IXLWorksheet worksheet, ReportRow row, IEnumerable<DateTime> dateRange, int rowIndex)
    {
        var currentDate = dateRange.Min();
        var dateTo = dateRange.Max();
        var columnIndex = 2;

        while (currentDate <= dateTo)
        {
            var value = row.Items.SingleOrDefault(x => x.Date == currentDate)?.Value;
            var valueCell = worksheet.Cell(rowIndex, columnIndex++);
            valueCell.SetValue(value == null ? "-" : $"{value} ₽");
            valueCell.Style.Fill.BackgroundColor = XLColor.Orange;

            currentDate = currentDate.AddDays(1);
        }

        var avgValueCell = worksheet.Cell(rowIndex, columnIndex++);
        avgValueCell.Value = row.AvgValue.ToString();
        avgValueCell.Style.Fill.BackgroundColor = XLColor.Yellow;

        var emojiValueCell = worksheet.Cell(rowIndex, columnIndex);
        emojiValueCell.Value = row.Emoji;
        emojiValueCell.Style.Fill.BackgroundColor = XLColor.Yellow;
    }

    #endregion

    private static byte[] ConvertWorkbookToByteArray(XLWorkbook workbook)
    {
        using var memoryStream = new MemoryStream();
        workbook.SaveAs(memoryStream);
        return memoryStream.ToArray();
    }
}
