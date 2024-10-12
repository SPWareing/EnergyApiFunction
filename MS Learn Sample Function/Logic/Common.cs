using Microsoft.Extensions.Logging;
using MS_Learn_Sample_Function.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MS_Learn_Sample_Function.Logic
{



    public class Common
    {
        private static readonly string userName = Environment.GetEnvironmentVariable("ApiKEY");
        private readonly string mpan = Environment.GetEnvironmentVariable("Elec_MPAN");
        private readonly string serial = Environment.GetEnvironmentVariable("Elec_SERIAL");
        private readonly string gasMprn = Environment.GetEnvironmentVariable("GAS_MPRN");
        private readonly string gasSerial = Environment.GetEnvironmentVariable("GAS_SERIAL");
        private readonly string accountNumber = Environment.GetEnvironmentVariable("Account_NO");
        private static readonly string uri = "https://api.octopus.energy/v1/";

        private static readonly HttpClient _client = MyClient();
        private static HttpClient MyClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(uri)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string password = "";
            var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var base64Credentials = Convert.ToBase64String(byteArray);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            return client;
        }

        /// <summary>
        /// Returns a Dictionary of the Dates
        /// </summary>
        /// <param name="dateFrom">Start Date </param>
        /// <param name="dateTo">End Date</param>
        /// <returns>Dictionary of <paramref name="dateFrom"/> and <paramref name="dateTo"/> in UTC format</returns>
        public static Dictionary<string, string> GetDates(string dateFrom, string dateTo)
        {
            string dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            return new Dictionary<string, string>()
            {
                { "period_from",DateTime.Parse(dateFrom).ToString(dateFormat) },
                { "period_to" ,DateTime.Parse(dateTo).ToString(dateFormat) },
                { "order_by" ,"period"},
                {"page_size","200"}
            };

        }


        public async Task<T> GetResultAsync<T>(string accountDetails, ILogger log, string dateFrom, string dateTo)
        {

            var requestBody = GetDates(dateFrom, dateTo);
            var queryString = string.Join("&", requestBody.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var request = $"{accountDetails}{queryString}";

            try
            {
                log.LogInformation($"Query String: {queryString}");
                log.LogInformation($"Request: {request}");
                var response = await _client.GetFromJsonAsync<T>(request);
                return response;
            }
            catch (Exception ex)
            {
                log.LogError($"Error fetching results: {ex.Message}");
                return default;
            }
        }
        /// <summary>
        /// Returns the Energy Consumption
        /// </summary>
        /// <param name="dateFrom">Start Date</param>
        /// <param name="dateTo">End Date</param>
        /// <param name="log">ILogger</param>
        /// <returns>An object of the Energy Consumption Class</returns>
        public async Task<EnergyConsumption> GetEnergyConsumption(string dateFrom, string dateTo, ILogger log)
        {
            string accountDetails = $"electricity-meter-points/{mpan}/meters/{serial}/consumption/?";
            return await GetResultAsync<EnergyConsumption>(accountDetails, log, dateFrom, dateTo);

        }
        /// <summary>
        /// Returns the Gas Consumption
        /// </summary>
        /// <param name="dateFrom">Start Date</param>
        /// <param name="dateTo">End Date</param>
        /// <param name="log"> ILogger</param>
        /// <returns> An Object of the Gas Consumption Class</returns>
        public async Task<GasConsumption> GetGasConsumption(string dateFrom, string dateTo, ILogger log)
        {
            string accountDetails = $"gas-meter-points/{gasMprn}/meters/{gasSerial}/consumption/?";
            return await GetResultAsync<GasConsumption>(accountDetails, log, dateFrom, dateTo);
        }



    }



}
