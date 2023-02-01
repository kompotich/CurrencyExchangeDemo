using CurrencyExchange.ExchangeData.ExchangeObjects;

namespace CurrencyExchange.Converter.Core;

public class ConverterService : IConverterService
{
    public IList<CurrencyRateItem> CalculateCurrencyRates(IEnumerable<CurrencyRateToRuble> rates) =>
        GetRubleCurrencyRates(rates).Concat(CalculateForeignCurrencyRates(rates)).ToList();

    private static IList<CurrencyRateItem> GetRubleCurrencyRates(IEnumerable<CurrencyRateToRuble> rates) =>
        (from source in rates
         join descination in rates on source.RequestDate equals descination.RequestDate
         select new CurrencyRateItem()
         {
             CurrencyRate = new CurrencyRateToRuble()
             {
                 CurrencyReferenceItem = new CurrencyReferenceItem()
                 {
                     Id = "R1000",
                     Name = "Российский рубль",
                     EngName = "Russian ruble",
                     ParentCode = "R1000",
                     Nominal = source.CurrencyReferenceItem.Nominal,
                     IsoCharCode = "RUB",
                     IsoNumCode = 643
                 }
             },
             BaseCurrencyRate = descination,
             RequestDate = source.RequestDate,
             Value = source.Value
         }).ToList();

    private static IList<CurrencyRateItem> CalculateForeignCurrencyRates(IEnumerable<CurrencyRateToRuble> rates) =>
        (from source in rates
         join descination in rates on source.RequestDate equals descination.RequestDate
         where source.CurrencyReferenceItem.Id != descination.CurrencyReferenceItem.Id
         let sourceValue = source.CurrencyReferenceItem.Nominal < descination.CurrencyReferenceItem.Nominal
            ? source.Value * descination.CurrencyReferenceItem.Nominal
            : source.Value
         let descinationValue = descination.CurrencyReferenceItem.Nominal < source.CurrencyReferenceItem.Nominal
            ? descination.Value * source.CurrencyReferenceItem.Nominal
            : descination.Value
         select new CurrencyRateItem
         {
             CurrencyRate = source,
             BaseCurrencyRate = descination,
             RequestDate = source.RequestDate,
             Value = descinationValue / sourceValue
         }).ToList();
}