using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class EnergyConsumptionClient
    {
        private readonly HttpClient _httpClient;
        public EnergyConsumptionClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://api.octopus.energy/v1/electricity-meter-points/");
        }

    }

}