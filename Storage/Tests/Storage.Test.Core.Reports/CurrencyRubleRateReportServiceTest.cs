using ClosedXML.Excel;
using CurrencyExchange.Storage.Core;
using CurrencyExchange.Storage.Core.Cache.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport;
using CurrencyExchange.Storage.Core.Reports.CurrencyRubleRateReport.Models;
using CurrencyExchange.Storage.Test.Core.Common.Reports;
using Moq;

namespace CurrencyExchange.Storage.Test.Core.Reports;

[TestClass]
public class CurrencyRubleRateReportServiceTest
{
    private readonly DateTime _dateTo;
    private readonly DateTime _dateFrom;

    private readonly ICurrencyRubleRateReportService _currencyRubleRateReportService;

    public CurrencyRubleRateReportServiceTest()
    {
        _currencyRubleRateReportService = new CurrencyRubleRateReportService(
            new Mock<ICurrencyRateService>().Object,
            new Mock<ICurrencyRubleRateReportCacheService>().Object);

        _dateTo = DateTime.Now.Date;
        _dateFrom = _dateTo.AddDays(-10);
    }

    [TestMethod]
    public void GenerateDocument_ReportContainsData_DocumentNotNull()
    {
        // arrange
        var report = CurrencyRubleRateReportDataSource.Get(_dateFrom, _dateTo);

        // act
        var document = _currencyRubleRateReportService.GenerateDocument(report);

        // assert
        Assert.IsNotNull(document);
    }

    [TestMethod]
    public void GenerateDocument_ReportContainsData_DocumentHasBytes()
    {
        // arrange
        var report = CurrencyRubleRateReportDataSource.Get(_dateFrom, _dateTo);

        // act
        var document = _currencyRubleRateReportService.GenerateDocument(report);

        // assert
        Assert.IsTrue(document.Any());
    }

    [TestMethod]
    public void GenerateDocument_EmptyReport_DocumentNotNull()
    {
        // arrange
        var report = new Report(Enumerable.Empty<ReportRow>());

        // act
        var document = _currencyRubleRateReportService.GenerateDocument(report);

        // assert
        Assert.IsNotNull(document);
    }

    [TestMethod]
    public void GenerateDocument_EmptyReport_DocumentHasBytes()
    {
        // arrange
        var report = new Report(Enumerable.Empty<ReportRow>());

        // act
        var document = _currencyRubleRateReportService.GenerateDocument(report);

        // assert
        Assert.IsTrue(document.Any());
    }

    [TestMethod]
    public void MakeWorkbook_HasSingleWorksheet()
    {
        // arrange
        var report = CurrencyRubleRateReportDataSource.Get(_dateFrom, _dateTo);

        // act
        using var workbook = MakeWorkbook(report);
        var worksheet = workbook.Worksheets.SingleOrDefault(x => x.Name == Report.PageName);

        // assert
        Assert.IsNotNull(worksheet);
    }

    [TestMethod]
    public void MakeWorkbook_ReportRowCountEqualsWorksheetContentRowCount()
    {
        // arrange
        var report = CurrencyRubleRateReportDataSource.Get(_dateFrom, _dateTo);

        // act
        using var workbook = MakeWorkbook(report);
        var worksheet = workbook.Worksheets.Single(x => x.Name == Report.PageName);

        // assert
        Assert.AreEqual(report.Rows.Count, worksheet.Rows().Count() - 2); // minus hat row and column title row
    }

    [TestMethod]
    public void MakeWorkbook_MaxReportRowItemsCountEqualsWorksheetColumnCount()
    {
        // arrange
        var report = CurrencyRubleRateReportDataSource.Get(_dateFrom, _dateTo);

        // act
        using var workbook = MakeWorkbook(report);
        var worksheet = workbook.Worksheets.Single(x => x.Name == Report.PageName);
        var expected = report.Rows.Max(x => x.Items.Count);
        var actual = worksheet.Columns().Count() - 3; // minus valute name column, avg value column, emoji column

        // assert
        Assert.AreEqual(expected, actual);
    }

    #region

    private XLWorkbook MakeWorkbook(Report report)
    {
        using var memoryStream = new MemoryStream(_currencyRubleRateReportService.GenerateDocument(report));

        return new XLWorkbook(memoryStream);
    }

    #endregion
}