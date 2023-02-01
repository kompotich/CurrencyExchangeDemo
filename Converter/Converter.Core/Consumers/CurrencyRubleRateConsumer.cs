using CurrencyExchange.Converter.Core;
using CurrencyExchange.ExchangeData.Queue;
using MassTransit;

namespace Converter.Core.Consumers;

public class CurrencyRubleRateConsumer : IConsumer<CurrencyRateToRubleList>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IConverterService _converterService;

    public CurrencyRubleRateConsumer(
        IPublishEndpoint publishEndpoint,
        IConverterService converterService)
    {
        _publishEndpoint = publishEndpoint;
        _converterService = converterService;
    }

    public async Task Consume(ConsumeContext<CurrencyRateToRubleList> context)
    {
        if (context.Message.Values?.Any() != true)
        {
            return;
        }

        var currencyRateQueueItem = new CurrencyRateItemList(
            _converterService.CalculateCurrencyRates(context.Message.Values))
        {
            LastProcessRequestDate = context.Message.LastProcessRequestDate
        };

        await _publishEndpoint.Publish(currencyRateQueueItem);
    }
}
