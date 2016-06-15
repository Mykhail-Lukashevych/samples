using MnbSoapApi;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null || currencies.ToArray().Length == 0)
            {
                yield break;
            }

            var client = new MnbSoapApiClient();
            var currencyNames = currencies.Select(c => c.Code);
            List<MnbDay> exchangeRatesByDays = client.GetExchangeRates(currencyNames.ToArray());
            List<MnbRate> exchangeRates = exchangeRatesByDays[0].Rates;

            for (int i = 0; i < exchangeRates.Count - 1; i++)
            {
                for (int j = i + 1; j < exchangeRates.Count; j++)
                {
                    MnbRate source = exchangeRates[i];
                    MnbRate target = exchangeRates[j];
                    decimal value = (decimal)(target.Unit * source.Value / (source.Unit * target.Value));
                    yield return new ExchangeRate(
                        new Currency(source.Currency),
                        new Currency(target.Currency),
                        value
                    );
                }
            }
        }
    }
}
