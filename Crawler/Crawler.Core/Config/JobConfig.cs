namespace CurrencyExchange.Crawler.Core.Config;

public class JobConfig
{
    public string? RecurringDailyJobCronExpression { get; set; }
    public int DefaultLoadingDebthYearAgo { get; set; }
}
