using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MS_Learn_Sample_Function.Logic
{

    

    public  class Common
    {
        private string userName = Environment.GetEnvironmentVariable("ApiKEY");
        private string mpan = Environment.GetEnvironmentVariable("Elec_MPAN");
        private string serial = Environment.GetEnvironmentVariable("Elec_SERIAL");
        public  async Task<EnergyConsumption> GetEnergyConsumption(HttpClient _client, string dateFrom, string dateTo, string energyType, ILogger log)
        {                        
            string password = "";           
            
            var byteArray = Encoding.ASCII.GetBytes($"{userName}:{password}");
            var base64Credentials = Convert.ToBase64String(byteArray);

            var client = _client;
            client.BaseAddress = new Uri("https://api.octopus.energy/v1/electricity-meter-points/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);
            string accountDetails = $"{mpan}/meters/{serial}/consumption/?";

            var requestBody = new Dictionary<string, string>()
            {
                { "period_from",DateTime.Parse(dateFrom).ToString("s") },
                { "period_to" ,DateTime.Parse(dateTo).ToString("s") },
                { "order_by" ,"period"},
                {"page_size","100"}
            };

            var queryString = string.Join("&", requestBody.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            log.LogInformation($"Query String: {queryString}");

            var request = $"{accountDetails}{queryString}";

            log.LogInformation($"Request: {request}");

            var response = await client.GetFromJsonAsync<EnergyConsumption>(request);
            return response;

        }

    }


    
}
