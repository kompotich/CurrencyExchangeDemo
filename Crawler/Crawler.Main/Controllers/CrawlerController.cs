using CurrencyExchange.Crawler.Core;
using CurrencyExchange.Crawler.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Crawler.Main.Controllers;

[Route("Crawler")]
[ApiController]
public class CrawlerController : ControllerBase
{
    private readonly ICrawlerService _crawlerService;
    
    public CrawlerController(ICrawlerService crawlerService)
    {
        _crawlerService = crawlerService;
    }

    [Route("CurrencyRubleReference")]
    [HttpGet]
    public async Task<IActionResult> GetCurrencyReferenceAsync()
    {
        var result = await _crawlerService.GetCurrencyReferenceAsync();
        return Ok(result);
    }

    [Route("CurrencyRatesToRuble/{date}")]
    [HttpGet]
    public async Task<JsonResult> GetCurrencyRatesToRubleAsync(DateTime date)
    {
        var rates = await _crawlerService.GetCurrencyRatesToRubleAsync(date);
        
        var result = rates.Select(x => new CurrencyRateModel()
        {
            Name = x.CurrencyReferenceItem.Name,
            EngName = x.CurrencyReferenceItem.EngName,
            IsoCharCode = x.CurrencyReferenceItem.IsoCharCode,
            Nominal = x.CurrencyReferenceItem.Nominal,
            RequestDate = x.RequestDate,
            Value = x.Value
        });

        return new JsonResult(rates);
    }
}
