using CurrencyExchange.Crawler.Core;
using Microsoft.AspNetCore.Mvc;

namespace Crawler.Main.Controllers;

[Route("CrawlerJob")]
[ApiController]
public class CrawlerJobController : ControllerBase
{
    private readonly ICrawlerJobService _crawlerJobService;

    public CrawlerJobController(ICrawlerJobService crawlerJobService)
    {
        _crawlerJobService = crawlerJobService;
    }

    [Route("RunCurrencyRubleReferenceJob")]
    [HttpGet]
    public ActionResult RunCurrencyRubleReferenceJob()
    {
        _crawlerJobService.RunCurrencyReferenceJob();
        return Ok($"CurrencyReferenceJob started at {DateTime.Now:dd.MM.yyyy HH:mm}");
    }

    [Route("RunCurrencyRubleRatesJob")]
    [HttpGet]
    public ActionResult RunCurrencyRubleRatesJob()
    {
        _crawlerJobService.RunCurrencyRatesToRubleJob();
        return Ok($"CurrencyRubleRatesJob started at {DateTime.Now:dd.MM.yyyy HH:mm}");
    }

    [Route("RunCurrencyRubleRatesRecurringJob")]
    [HttpGet]
    public ActionResult RunCurrencyRubleRatesRecurringJob()
    {
        _crawlerJobService.RunCurrencyRatesToRubleRecurringJob();
        return Ok($"CurrencyRubleRatesRecurringJob started at {DateTime.Now:dd.MM.yyyy HH:mm}");
    }
}
