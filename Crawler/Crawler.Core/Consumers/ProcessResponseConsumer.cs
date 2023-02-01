using CurrencyExchange.Crawler.Database;
using CurrencyExchange.ExchangeData.Queue;
using MassTransit;

namespace CurrencyExchange.Crawler.Core.Consumers
{
    public class ProcessResponseConsumer : IConsumer<ProcessResponse>
    {
        ISettingRepository _settingRepository;

        public ProcessResponseConsumer(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task Consume(ConsumeContext<ProcessResponse> context)
        {
            if (context.Message.Errors?.Any() != true) 
            {
                return;
            }

            var requestDateSetting = await _settingRepository.GetSettingAsync(Common.Constants.NextRequestDateSettingName);
            requestDateSetting.Value = context.Message.LastProcessRequestDate.ToString();
            await _settingRepository.UpdateAsync(requestDateSetting);
        }
    }
}
