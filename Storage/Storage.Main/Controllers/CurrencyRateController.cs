using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.Storage.Core;
using CurrencyExchange.Storage.Core.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Storage.Main.Controllers;

[Route("Storage/CurrencyRate")]
public class CurrencyRateController : ControllerBase
{
    private readonly ICurrencyRateService _currencyRateService;

    public CurrencyRateController(ICurrencyRateService currencyRateService)
    {
        _currencyRateService = currencyRateService;
    }

    [Route("Add")]
    [HttpPost]
    public async Task<IActionResult> Add(CurrencyRateItem currencyRateItem)
    {
        await _currencyRateService.AddAsync(currencyRateItem);
        return Ok();
    }

    [Route("AddRange")]
    [HttpPost]
    public async Task<IActionResult> AddRange(IEnumerable<CurrencyRateItem> currencyRates)
    {
        await _currencyRateService.AddRangeAsync(currencyRates);
        return Ok();
    }

    [Route("")]
    [HttpPost]
    public async Task<IActionResult> Get(CurrencyRateFilter filter) => 
        Ok(await _currencyRateService.GetAsync(filter));
}
