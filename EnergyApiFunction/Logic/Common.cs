using Energy_Consumption_Function.Classes;
using Energy_Consumption_Function.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Logic
{
    /// <summary>
    /// Provides common functionality for interacting with the Octopus Energy API.
    /// </summary>
    public class Common
    {        
        private readonly string mpan = Environment.GetEnvironmentVariable("Elec_MPAN");
        private readonly string serial = Environment.GetEnvironmentVariable("Elec_SERIAL");
        private readonly string gasMprn = Environment.GetEnvironmentVariable("GAS_MPRN");
        private readonly string gasSerial = Environment.GetEnvironmentVariable("GAS_SERIAL");
        private readonly string accountNumber = Environment.GetEnvironmentVariable("Account_NO");
        private static readonly string uri = "https://api.octopus.energy/v1/";

        private readonly HttpClient _client;
        private readonly ILogger _log;
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClient"/> class.
        /// </summary>
        /// <returns>An initialized <see cref="HttpClient"/> instance.</returns>
        public Common(HttpClient client, ILogger log)
        {
            _client = client;
            _log = log;
        }     

        /// <summary>
        /// Fetches the result from the Octopus Energy API.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="accountDetails">The account details for the request.</param>  
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>
        /// <returns>The result of the request.</returns>

        public async Task<T?> GetResultAsync<T>(string accountDetails, string dateFrom, string dateTo)
        {
            var requestBody = HelperFunctions.GetDates(dateFrom, dateTo);
            var queryString = string.Join("&", requestBody.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            var request = $"{accountDetails}{queryString}";

            try
            {
                _log.LogInformation("Query String: {QueryString}", queryString);
                _log.LogInformation("Request: {Request}", request);
                var response = await _client.GetFromJsonAsync<T>(request);
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching results for request: {Request}", request);
                return default;
            }
        }
        
        /// <summary>
        /// Returns the energy consumption.
        /// </summary>
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>       
        /// <returns>An object of the <see cref="Consumption"/> class.</returns>
        public async Task<Consumption?> GetConsumption(string dateFrom, string dateTo, TariffType tariffType)
        {
            
            var meterType = tariffType switch
            {
                TariffType.Electricity => new  MeterType{ Type = "electricity", Mpan = mpan, Serial = serial },
                TariffType.Gas => new MeterType { Type = "gas", Mpan = gasMprn, Serial = gasSerial },
                _ => throw new ArgumentOutOfRangeException(nameof(tariffType), tariffType, null)
            };

            return
                await GetResultAsync<Consumption>(ComposeConsumption(meterType), dateFrom, dateTo);
        }

        private static string ComposeConsumption(MeterType meterType) => $"{meterType.Type}-meter-points/{meterType.Mpan}/meters/{meterType.Serial}/consumption/?";

        /// <summary>
        /// Returns the Tariff .
        /// </summary>
        /// <param name="dateFrom">The start date for the request.</param>
        /// <param name="dateTo">The end date for the request.</param>
        /// <param name="tariffCode">The tariff code.</param>        

        public Task<Tariff?>? GetTariff(string dateFrom, string dateTo, string tariffCode, TariffType tarifftype)
        {
            try
            {
                var tariffTypeString = TariffTypeExtension.ToFriendlyString(tarifftype);
                var baseCode = HelperFunctions.GetTariffCode(tariffCode, _log);

                string accountDetails = $"products/{baseCode}/{tariffTypeString}/{tariffCode}/standard-unit-rates/?";
                return GetResultAsync<Tariff>(accountDetails, dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching tariff for code: {TariffCode}", tariffCode);
                return default;
            }
        }

        public async Task<AccountDetails?> GetAccountDetails()
        {
            try
            {
                string accountDetails = $"accounts/{accountNumber}/";
                return await _client.GetFromJsonAsync<AccountDetails>(accountDetails);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Error fetching account details for account number: {AccountNumber}", accountNumber);
                throw;
            }
        }


    }



}
