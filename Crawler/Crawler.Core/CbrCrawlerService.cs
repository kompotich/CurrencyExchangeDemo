using Crawler.Core.Contracts.Cbr;
using CurrencyExchange.ExchangeData.ExchangeObjects;
using Microsoft.Extensions.Options;
using System.Xml.Serialization;

namespace CurrencyExchange.Crawler.Core;

public class CbrCrawlerService : ICrawlerService
{
    private readonly Config.Config _config;

    private readonly HttpClient _httpClient;

    public CbrCrawlerService(IOptions<Config.Config> config)
    {
        _config = config.Value;

        _httpClient = new HttpClient();
    }

    public async Task<IList<CurrencyReferenceItem>> GetCurrencyReferenceAsync()
    {
        var httpResponseMessage = await _httpClient.GetAsync(_config.ExternalApi.Cbr.CurrencyReferenceEndpoint);
        using var xmlStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        
        if (new XmlSerializer(typeof(ForeignCurrencyMarketLibItemList)).Deserialize(xmlStream) is not
            ForeignCurrencyMarketLibItemList foreignCurrencyMarketLibItemList)
        {
            throw new InvalidCastException();
        }

        var currencyRubleInfos = foreignCurrencyMarketLibItemList.ForeignCurrencyMarketLibItems.Select(x => new CurrencyReferenceItem()
        {
            Id = x.Id,
            Name = x.Name,
            EngName = x.EngName,
            Nominal = Convert.ToInt32(x.Nominal),
            ParentCode = x.ParentCode,
            IsoNumCode = string.IsNullOrEmpty(x.IsoNumCode) ? 0 : Convert.ToInt32(x.IsoNumCode), // todo: make nullable
            IsoCharCode = x.IsoCharCode.TrimEnd()
        }).ToList();

        currencyRubleInfos.Add(new CurrencyReferenceItem()
        {
            Id = "R1000",
            Name = "Российский рубль",
            EngName = "Russian ruble",
            Nominal = 1,
            ParentCode = "R1000",
            IsoNumCode = 643,
            IsoCharCode = "RUB"
        });

        return currencyRubleInfos;
    }

    public async Task<IList<CurrencyRateToRuble>> GetCurrencyRatesToRubleAsync(DateTime date)
    {
        var request = string.Concat(_config.ExternalApi.Cbr.CurrencyRatesEndpoint, date.ToString("dd.MM.yyyy"));
        var httpResponseMessage = await _httpClient.GetAsync(request);
        using var xmlStream = await httpResponseMessage.Content.ReadAsStreamAsync();
        
        if (new XmlSerializer(typeof(ForeignCurrencyMarketItemList)).Deserialize(xmlStream) is not
            ForeignCurrencyMarketItemList foreignCurrencyMarketItemList)
        {
            throw new InvalidCastException();
        }

        return foreignCurrencyMarketItemList.ForeignCurrencyMarketItems.Select(x => new CurrencyRateToRuble()
        {
            CurrencyReferenceItem = new CurrencyReferenceItem()
            {
                Id = x.Id,
                IsoNumCode = string.IsNullOrEmpty(x.NumCode) ? 0 : Convert.ToInt32(x.NumCode), // todo: make nullable
                IsoCharCode = x.CharCode.TrimEnd(),
                Nominal = Convert.ToInt32(x.Nominal),
                Name = x.Name
            },
            Value = Convert.ToDecimal(x.Value),
            RequestDate = date
        }).ToList();
    }
}
