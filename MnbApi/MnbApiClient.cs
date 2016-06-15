using MnbSoapApi.MnbSoap;
using System;
using System.Collections.Generic;

namespace MnbSoapApi
{
    public class MnbSoapApiClient
    {
        public List<MnbDay> GetExchangeRates(string[] currencyNames)
        {
            if (currencyNames == null || currencyNames.Length == 0)
            {
                return new List<MnbDay>(0);
            }

            var client = new MNBArfolyamServiceSoapClient();
            List<MnbDay> days = null;
            try
            {
                var requestBody = new GetExchangeRatesRequestBody();
                requestBody.startDate = DateTime.Today.AddDays(-5.0).ToString();
                requestBody.endDate = DateTime.Today.ToString();
                requestBody.currencyNames = string.Join(",", currencyNames);
                GetExchangeRatesResponseBody responseBody = client.GetExchangeRates(requestBody);

                var parser = new MnbResponseParser();
                days = parser.Parse(responseBody.GetExchangeRatesResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                client.Close();
            }
            return days;
        }
    }
}
