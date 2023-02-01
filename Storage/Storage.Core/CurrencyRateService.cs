using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.Storage.Core.Filters;
using CurrencyExchange.Storage.Core.Models;
using CurrencyExchange.Storage.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CurrencyExchange.Storage.Core;

public class CurrencyRateService : ICurrencyRateService
{
    private readonly ICurrencyExchangeRepository _currencyExchangeRepository;
    
    public CurrencyRateService(ICurrencyExchangeRepository currencyExchangeRepository)
    {
        _currencyExchangeRepository = currencyExchangeRepository;
    }

    public async Task AddAsync(CurrencyRateItem currencyRate)
    {
        if (currencyRate is null)
        {
            throw new ArgumentNullException(nameof(currencyRate));
        }

        var references = await _currencyExchangeRepository.CurrencyReference
            .GetByCondition(x => x.IsoCharCode == currencyRate.CurrencyRate.CurrencyReferenceItem.IsoCharCode
                || x.IsoCharCode == currencyRate.BaseCurrencyRate.CurrencyReferenceItem.IsoCharCode)
            .ToDictionaryAsync(x =>
                x.IsoCharCode ?? throw new NullReferenceException(),
                x => x.Id);

        _currencyExchangeRepository.CurrencyRate.Add(new Database.Entities.Models.CurrencyRate()
        {
            CurrencyReferenceId = references[currencyRate.CurrencyRate.CurrencyReferenceItem.IsoCharCode],
            BaseCurrencyReferenceId = references[currencyRate.BaseCurrencyRate.CurrencyReferenceItem.IsoCharCode],
            RequestDate = currencyRate.RequestDate.ToUniversalTime(),
            Value = currencyRate.Value
        });

        await _currencyExchangeRepository.SaveAsync();
    }

    public async Task AddRangeAsync(IEnumerable<CurrencyRateItem> currencyRates)
    {
        if (currencyRates is null)
        {
            throw new ArgumentNullException(nameof(currencyRates));
        }

        if (!currencyRates.Any())
        {
            return;
        }

        var references = await _currencyExchangeRepository.CurrencyReference
            .GetAll()
            .ToDictionaryAsync(x => x.IsoCharCode ?? throw new NullReferenceException(), x => x.Id);

        _currencyExchangeRepository.CurrencyRate.AddRange(
            currencyRates.Select(x => new Database.Entities.Models.CurrencyRate()
            {
                CurrencyReferenceId = references[x.CurrencyRate.CurrencyReferenceItem.IsoCharCode],
                BaseCurrencyReferenceId = references[x.BaseCurrencyRate.CurrencyReferenceItem.IsoCharCode],
                RequestDate = x.RequestDate.ToUniversalTime(),
                Value = x.Value
            }));

        await _currencyExchangeRepository.SaveAsync();
    }

    public async Task<IList<CurrencyRateModel>> GetAsync(CurrencyRateFilter filter) =>
        await _currencyExchangeRepository.CurrencyRate.GetByCondition(MakeExpression(filter))
            .Select(x => new CurrencyRateModel()
            {
                CurrencyName = x.CurrencyReference.Name,
                CurrencyEngName = x.CurrencyReference.EngName,
                CurrencyIsoCharCode = x.CurrencyReference.IsoCharCode,
                BaseCurrencyName = x.BaseCurrencyReference.Name,
                BaseCurrencyEngName = x.BaseCurrencyReference.EngName,
                BaseCurrencyIsoCharCode = x.BaseCurrencyReference.IsoCharCode,
                RequestDate = x.RequestDate,
                Value = x.Value,
            }).ToListAsync();

    private static Expression<Func<Database.Entities.Models.CurrencyRate, bool>> MakeExpression(CurrencyRateFilter filter) =>
        x => filter.RequestDateFrom.HasValue ? x.RequestDate >= filter.RequestDateFrom.Value.ToUniversalTime() : true &&
            filter.RequestDateTo.HasValue ? x.RequestDate <= filter.RequestDateTo.Value.ToUniversalTime() : true &&
            filter.IsoCharCodes != null && filter.IsoCharCodes.Any() ? filter.IsoCharCodes.Contains(x.CurrencyReference.IsoCharCode) : true;
}
