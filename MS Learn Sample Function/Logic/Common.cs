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
        private static string userName = Environment.GetEnvironmentVariable("ApiKEY");
        private string mpan = Environment.GetEnvironmentVariable("Elec_MPAN");
        private string serial = Environment.GetEnvironmentVariable("Elec_SERIAL");
        private string gasMprn = Environment.GetEnvironmentVariable("GAS_MPRN");
        private string gasSerial = Environment.GetEnvironmentVariable("GAS_SERIAL");
        private string accountNumber = Environment.GetEnvironmentVariable("Account_NO");
        private static string uri = "https://api.octopus.energy/v1/";

        private static HttpClient MyClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string password = "";
            var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var base64Credentials = Convert.ToBase64String(byteArray);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            return client;
        }

        public static Dictionary<string, string> GetDates(string dateFrom, string dateTo)
        {
            string dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            var reqs = new Dictionary<string, string>()
            {
                { "period_from",DateTime.Parse(dateFrom).ToString(dateFormat) },
                { "period_to" ,DateTime.Parse(dateTo).ToString(dateFormat) },
                { "order_by" ,"period"},
                {"page_size","200"}
            };
            return reqs;
        }


        public async Task<EnergyConsumption> GetEnergyConsumption(string dateFrom, string dateTo, string energyType, ILogger log)
        {
            /*string password = "";           
            
            var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var base64Credentials = Convert.ToBase64String(byteArray);

            var client = _client;
            client.BaseAddress = new Uri("https://api.octopus.energy/v1/electricity-meter-points/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);*/
            var client = MyClient();
            string accountDetails = $"electricity-meter-points/{mpan}/meters/{serial}/consumption/?";

            var requestBody = GetDates(dateFrom, dateTo);

            var queryString = string.Join("&", requestBody.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            log.LogInformation($"Query String: {queryString}");

            var request = $"{accountDetails}{queryString}";

            log.LogInformation($"Request: {request}");


            var response = await client.GetFromJsonAsync<EnergyConsumption>(request);
            return response;

        }

        public async Task<GasConsumption> GetGasConsumption(string dateFrom, string dateTo, ILogger log)
        {
            var client = MyClient();
            string accountDetails = $"gas-meter-points/{gasMprn}/meters/{gasSerial}/consumption/?";

            var requestBody = GetDates(dateFrom, dateTo);

            var queryString = string.Join("&", requestBody.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            log.LogInformation($"Query String: {queryString}");

            var request = $"{accountDetails}{queryString}";

            log.LogInformation($"Request: {request}");

            var response = await client.GetFromJsonAsync<GasConsumption>(request);

            log.LogInformation($"GAS Response: {response}");
            return response;

        }



    }



}
