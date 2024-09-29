using System;
using System.Net.Http;

namespace MS_Learn_Sample_Function.Classes
{
    public class Energy
    {

        public DateTime from { get; set; }
        public DateTime to { get; set; }
        public string energyType { get; set; }

    }

    public class GasConsumption
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public Result[] results { get; set; }
    }



    public class EnergyConsumption
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public Result[] results { get; set; }
    }

    public class Result
    {
        public float consumption { get; set; }
        public DateTime interval_start { get; set; }
        public DateTime interval_end { get; set; }
    }


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
