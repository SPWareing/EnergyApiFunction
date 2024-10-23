using Microsoft.Extensions.Logging;
using Energy_Consumption_Function.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Energy_Consumption_Function.Logic
{

    /// <summary>
    /// Provides common functionality for interacting with the Octopus Energy API.
    /// </summary>
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
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        /// </summary>
        /// <returns>An initialized <see cref="HttpClient"/> instance.</returns>
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
        /// Returns a dictionary of the dates in UTC format.
        /// </summary>
        /// <param name="dateFrom">Start date.</param>
        /// <param name="dateTo">End date.</param>
        /// <returns>A dictionary containing the start and end dates in UTC format.</returns>
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
        /// <summary>
        /// Fetches the result from the Octopus Energy API.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="accountDetails">The account details for the request.</param>
        /// <param name="log">The logger instance.</param>
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>
        /// <returns>The result of the request.</returns>

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
        /// Returns the energy consumption.
        /// </summary>
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An object of the <see cref="EnergyConsumption"/> class.</returns>
        public async Task<EnergyConsumption> GetEnergyConsumption(string dateFrom, string dateTo, ILogger log)
        {
            string accountDetails = $"electricity-meter-points/{mpan}/meters/{serial}/consumption/?";
            return await GetResultAsync<EnergyConsumption>(accountDetails, log, dateFrom, dateTo);

        }
        /// <summary>
        /// Returns the gas consumption.
        /// </summary>
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An object of the <see cref="GasConsumption"/> class.</returns>
        public async Task<GasConsumption> GetGasConsumption(string dateFrom, string dateTo, ILogger log)
        {
            string accountDetails = $"gas-meter-points/{gasMprn}/meters/{gasSerial}/consumption/?";
            return await GetResultAsync<GasConsumption>(accountDetails, log, dateFrom, dateTo);
        }


        /// <summary>
        /// Returns the Tariff .
        /// </summary>
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>
        /// <param name="tariffCode">The tariff code.</param>
        /// <param name="log">The logger instance.</param>
        /// <returns>An object of either <see cref="GasTariff"/>  or  <see cref="ElecTariff"/> class.</returns>
        public Task<T> GetTariff<T>(string dateFrom, string dateTo, string tariffCode, string tarifftype, ILogger log) {

            try
            { 
            var baseCode = GetTariffCode(tariffCode, log);

            string accountDetails = $"products/{baseCode}/{tarifftype}/{tariffCode}/standard-unit-rates/?";
            return GetResultAsync<T>(accountDetails, log, dateFrom, dateTo);
            }
            catch(Exception ex)
            {
                log.LogError($"Error fetching gas tariff: {ex.Message}");
                return default;
            }
        }

        public async Task<AccountDetails>GetAccountDetails(ILogger log)
        {
            try
            {
                string accountDetails = $"accounts/{accountNumber}/";
                return await _client.GetFromJsonAsync<AccountDetails>(accountDetails);
            }
            catch(Exception ex)
            {
                log.LogError($"Error fetching account details: {ex.Message}");
                return default;
            }
        }


        public string  GetTariffCode( string tariffCode, ILogger log)
        {
            var rgx = new Regex(@"[A-Z]{3}-\d{2}-\d{2}-\d{2}");

            if (rgx.IsMatch(tariffCode))
            {
                return rgx.Matches(tariffCode)[0].Value;
            }
            else
            {
                log.LogError("Invalid Tariff Code");
                return string.Empty;
            }

            
        }

    }



}
