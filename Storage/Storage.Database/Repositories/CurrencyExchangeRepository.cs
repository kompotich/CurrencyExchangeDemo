using CurrencyExchange.Storage.Database.Entities;

namespace CurrencyExchange.Storage.Database.Repositories;

public class CurrencyExchangeRepository : ICurrencyExchangeRepository
{
    private readonly CurrencyExchangeContext _currencyExchangeContext;
    private readonly ICurrencyReferenceRepository _currencyReferenceRepository;
    private readonly ICurrencyRateRepository _currencyRateRepository;

    public CurrencyExchangeRepository(
        CurrencyExchangeContext currencyExchangeContext,
        ICurrencyReferenceRepository currencyReferenceRepository,
        ICurrencyRateRepository currencyRateRepository)
    {
        _currencyExchangeContext = currencyExchangeContext;
        _currencyReferenceRepository = currencyReferenceRepository;
        _currencyRateRepository = currencyRateRepository;
    }

    public ICurrencyReferenceRepository CurrencyReference => _currencyReferenceRepository;

    public ICurrencyRateRepository CurrencyRate => _currencyRateRepository;

    public async Task SaveAsync() => await _currencyExchangeContext.SaveChangesAsync();

}
