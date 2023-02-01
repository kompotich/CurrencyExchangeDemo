using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.Storage.Core;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Storage.Main.Controllers;

[Route("Storage/CurrencyReference")]
public class CurrencyReferenceController : ControllerBase
{
    private readonly ICurrencyReferenceService _currencyReferenceService;

    public CurrencyReferenceController(ICurrencyReferenceService currencyReferenceService)
    {
        _currencyReferenceService = currencyReferenceService;
    }

    [Route("Add")]
    [HttpPost]
    public async Task<IActionResult> Add(CurrencyReferenceItem currencyReferenceItem)
    {
        await _currencyReferenceService.AddAsync(currencyReferenceItem);
        return Ok();
    }

    [Route("AddRange")]
    [HttpPost]
    public async Task<IActionResult> AddRange(IEnumerable<CurrencyReferenceItem> currencyReferenceItem)
    {
        await _currencyReferenceService.AddRangeAsync(currencyReferenceItem);
        return Ok();
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _currencyReferenceService.GetAllAsync());
}
