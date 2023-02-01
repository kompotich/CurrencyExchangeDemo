using CurrencyExchange.ExchangeData.Queue;
using MassTransit;

namespace CurrencyExchange.Storage.Core.Consumers;

public class CurrencyReferenceConsumer : IConsumer<CurrencyReferenceItemList>
{
    private readonly ICurrencyReferenceService _currencyReferenceService;

    public CurrencyReferenceConsumer(ICurrencyReferenceService currencyReferenceService)
    {
        _currencyReferenceService = currencyReferenceService;
    }

    public async Task Consume(ConsumeContext<CurrencyReferenceItemList> context) =>
        await _currencyReferenceService.UpdateRangeAsync(context.Message.Values);
}
