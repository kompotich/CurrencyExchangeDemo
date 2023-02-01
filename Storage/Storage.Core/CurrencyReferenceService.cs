using CurrencyExchange.Storage.Core.Models;
using CurrencyExchange.Storage.Database.Repositories;
using CurrencyExchange.ExchangeData.ExchangeObjects;
using Microsoft.EntityFrameworkCore;
using CurrencyExchange.Storage.Database.Entities.Models;

namespace CurrencyExchange.Storage.Core;

public class CurrencyReferenceService : ICurrencyReferenceService
{
    private readonly ICurrencyExchangeRepository _currencyExchangeRepository;

    public CurrencyReferenceService(ICurrencyExchangeRepository currencyExchangeRepository)
    {
        _currencyExchangeRepository = currencyExchangeRepository;
    }

    public async Task AddAsync(CurrencyReferenceItem currencyRubleInfo)
    {
        if (currencyRubleInfo is null)
        {
            throw new ArgumentNullException(nameof(currencyRubleInfo));
        }

        _currencyExchangeRepository.CurrencyReference.Add(new CurrencyReference()
        {
            Name = currencyRubleInfo.Name,
            EngName = currencyRubleInfo.EngName,
            IsoCharCode = currencyRubleInfo.IsoCharCode,
            ParentCode = currencyRubleInfo.ParentCode
        });

        await _currencyExchangeRepository.SaveAsync();
    }

    public async Task AddRangeAsync(IEnumerable<CurrencyReferenceItem> currencyRubleInfos)
    {
        if (currencyRubleInfos is null)
        {
            throw new ArgumentNullException(nameof(currencyRubleInfos));
        }

        if (!currencyRubleInfos.Any())
        {
            return;
        }

        _currencyExchangeRepository.CurrencyReference.AddRange(
            currencyRubleInfos.Select(x => new CurrencyReference()
            {
                Name = x.Name,
                EngName = x.EngName,
                IsoCharCode = x.IsoCharCode,
                ParentCode = x.ParentCode
            }));

        await _currencyExchangeRepository.SaveAsync();
    }

    public async Task<IList<CurrencyReferenceModel>> GetAllAsync() =>
        await _currencyExchangeRepository.CurrencyReference
            .GetAll()
            .Select(x => new CurrencyReferenceModel
            {
                Name = x.Name,
                EngName = x.EngName,
                ParentCode = x.ParentCode,
                IsoCharCode = x.IsoCharCode,
            }).ToListAsync();

    public async Task UpdateAsync(CurrencyReferenceItem currencyRubleInfo)
    {
        if (currencyRubleInfo is null)
        {
            throw new ArgumentNullException(nameof(currencyRubleInfo));
        }

        var entity = await _currencyExchangeRepository.CurrencyReference
            .GetByCondition(x => x.IsoCharCode == currencyRubleInfo.IsoCharCode)
            .SingleAsync();

        entity.Name = currencyRubleInfo.Name;
        entity.EngName = currencyRubleInfo.EngName;
        entity.ParentCode = currencyRubleInfo.ParentCode;

        _currencyExchangeRepository.CurrencyReference.Update(entity);
        
        await _currencyExchangeRepository.SaveAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<CurrencyReferenceItem> currencyRubleInfos)
    {
        if (currencyRubleInfos is null)
        {
            throw new ArgumentNullException(nameof(currencyRubleInfos));
        }

        if (!currencyRubleInfos.Any())
        {
            return;
        }

        var entities =
            (from source in await _currencyExchangeRepository.CurrencyReference.GetAll().ToListAsync()
             join descination in currencyRubleInfos on source.IsoCharCode equals descination.IsoCharCode
             select new CurrencyReference()
             {
                 Id = source.Id,
                 Name = descination.Name,
                 EngName = descination.EngName,
                 ParentCode = descination.ParentCode,
                 IsoCharCode = source.IsoCharCode
             }).ToList();

        _currencyExchangeRepository.CurrencyReference.UpdateRange(entities);
        
        await _currencyExchangeRepository.SaveAsync();
    }
}
