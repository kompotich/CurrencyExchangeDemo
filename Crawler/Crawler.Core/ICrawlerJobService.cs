namespace CurrencyExchange.Crawler.Core;

public interface ICrawlerJobService
{
    void RunStartupJobs();

    void RunCurrencyReferenceJob();

    void RunCurrencyRatesToRubleJob();

    void RunCurrencyRatesToRubleRecurringJob();
}
