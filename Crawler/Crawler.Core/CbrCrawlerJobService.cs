using CurrencyExchange.Crawler.Database;
using CurrencyExchange.ExchangeData.ExchangeObjects;
using CurrencyExchange.ExchangeData.Queue;
using Hangfire;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace CurrencyExchange.Crawler.Core;

public class CbrCrawlerJobService : ICrawlerJobService
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ICrawlerService _crawlerService;
    private readonly ISettingRepository _settingRepository;
    private readonly Config.Config _config;

    public CbrCrawlerJobService(
        IRecurringJobManager recurringJobManager,
        IPublishEndpoint publishEndpoint,
        ICrawlerService crawlerService,
        ISettingRepository settingRepository,
        IOptions<Config.Config> config)
    {
        _recurringJobManager = recurringJobManager;
        _publishEndpoint = publishEndpoint;
        _crawlerService = crawlerService;
        _settingRepository = settingRepository;
        _config = config.Value;
    }

    public void RunStartupJobs()
    {
        RunCurrencyReferenceJob();
        RunCurrencyRatesToRubleRecurringJob();
    }

    public void RunCurrencyReferenceJob() =>
        BackgroundJob.Enqueue(() => ProcessCurrencyRubleReferenceAsync());

    public void RunCurrencyRatesToRubleJob()
    {
        var requestDateSetting = GetRequestDateSetting().Result;
        BackgroundJob.Enqueue(() => ProcessCurrencyRubleRatesByDateAsync(
            Convert.ToDateTime(requestDateSetting.Value)));
        SetTomorrowRequestDateSetting(requestDateSetting);
    }

    public void RunCurrencyRatesToRubleRecurringJob()
    {
        var requestDateSetting = GetRequestDateSetting().Result;
        _recurringJobManager.AddOrUpdate(
            $"currencyRatesRecurringJob",
            () => ProcessCurrencyRubleRatesByDateAsync(Convert.ToDateTime(requestDateSetting.Value)),
            _config.Job.RecurringDailyJobCronExpression);
        SetTomorrowRequestDateSetting(requestDateSetting);
    }

    //

    public async Task ProcessCurrencyRubleReferenceAsync() =>
        await _publishEndpoint.Publish(new CurrencyReferenceItemList(
            await _crawlerService.GetCurrencyReferenceAsync()));

    public async Task ProcessCurrencyRubleRatesByDateAsync(DateTime requestDate)
    {
        var lastRequestDate = requestDate;
        var rates = new List<CurrencyRateToRuble>();

        while (requestDate <= DateTime.Now.Date)
        {
            rates.AddRange(await _crawlerService.GetCurrencyRatesToRubleAsync(requestDate));
            requestDate = requestDate.AddDays(1);
        }

        await _publishEndpoint.Publish(new CurrencyRateToRubleList(rates)
        {
            LastProcessRequestDate = lastRequestDate
        });
    }

    //

    private async Task<Database.Entities.Models.Setting> GetRequestDateSetting()
    {
        var requestDateSetting = await _settingRepository.GetSettingAsync(Common.Constants.NextRequestDateSettingName);

        requestDateSetting ??= new Database.Entities.Models.Setting()
        {
            Name = Common.Constants.NextRequestDateSettingName,
            Type = Database.Entities.Models.SettingValueType.DateTime,
            Value = DateTime.Now.Date.AddYears(
                    -_config.Job.DefaultLoadingDebthYearAgo).ToString("dd.MM.yyyy")
        };

        return requestDateSetting;
    }

    private void SetTomorrowRequestDateSetting(Database.Entities.Models.Setting setting)
    {
        setting.Value = DateTime.Now.Date.AddDays(1).ToString("dd.MM.yyyy");
        _settingRepository.Update(setting);
    }
}
