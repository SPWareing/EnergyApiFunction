using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    /// <summary>
    /// Represents energy data with a date range and energy type.
    /// </summary>
    public class Energy
    {
        /// <summary>
        /// Gets or sets the start date of the energy data.
        /// </summary>
        public DateTime from { get; set; }

        /// <summary>
        /// Gets or sets the end date of the energy data.
        /// </summary>
        public DateTime to { get; set; }

        /// <summary>
        /// Gets or sets the type of energy.
        /// </summary>
        public string energyType { get; set; }
    }
    /// <summary>
    /// Represents gas consumption data.
    /// </summary>
    public class GasConsumption
    {
        /// <summary>
        /// Gets or sets the count of gas consumption records.
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// Gets or sets the URL for the next set of gas consumption records.
        /// </summary>
        public string next { get; set; }

        /// <summary>
        /// Gets or sets the URL for the previous set of gas consumption records.
        /// </summary>
        public object previous { get; set; }

        /// <summary>
        /// Gets or sets the array of gas consumption results.
        /// </summary>
        public Result[] results { get; set; }
    }

    /// <summary>
    /// Represents energy consumption data.
    /// </summary>
    public class EnergyConsumption
    {
        /// <summary>
        /// Gets or sets the count of energy consumption records.
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// Gets or sets the URL for the next set of energy consumption records.
        /// </summary>
        public string next { get; set; }

        /// <summary>
        /// Gets or sets the URL for the previous set of energy consumption records.
        /// </summary>
        public object previous { get; set; }

        /// <summary>
        /// Gets or sets the array of energy consumption results.
        /// </summary>
        public Result[] results { get; set; }
    }

    /// <summary>
    /// Represents a result of energy or gas consumption.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Gets or sets the consumption value.
        /// </summary>
        public float consumption { get; set; }

        /// <summary>
        /// Gets or sets the start time of the consumption interval.
        /// </summary>
        public DateTime interval_start { get; set; }

        /// <summary>
        /// Gets or sets the end time of the consumption interval.
        /// </summary>
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

    public interface ITodoClient
    {
    }

    public class TodoClient : ITodoClient
    {
        private readonly HttpClient _httpClient;
        public TodoClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/todos/1");
        }

        public async Task<Todo> GetTodo()
        {
            var response = await _httpClient.GetAsync("");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Todo>(content);
        }
    }

    public class Todo
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }

    public class GasTariff
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultTariff[] results { get; set; }
    }
    public class ElecTariff
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultTariff[] results { get; set; }
    }

    public class ResultTariff
    {
        public float value_exc_vat { get; set; }
        public float value_inc_vat { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime? valid_to { get; set; }
        public string payment_method { get; set; }
    }


    public class AccountDetails
    {
        public string number { get; set; }
        public Property1[] properties { get; set; }
    }

    public class Property1
    {
        public int id { get; set; }
        public DateTime moved_in_at { get; set; }
        public object moved_out_at { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string address_line_3 { get; set; }
        public string town { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public Electricity_Meter_Points[] electricity_meter_points { get; set; }
        public Gas_Meter_Points[] gas_meter_points { get; set; }
    }

    public class Electricity_Meter_Points
    {
        public string mpan { get; set; }
        public int profile_class { get; set; }
        public int consumption_standard { get; set; }
        public Meter[] meters { get; set; }
        public Agreement[] agreements { get; set; }
        public bool is_export { get; set; }
    }

    public class Meter
    {
        public string serial_number { get; set; }
        public Register[] registers { get; set; }
    }

    public class Register
    {
        public string identifier { get; set; }
        public string rate { get; set; }
        public bool is_settlement_register { get; set; }
    }

    public class Agreement
    {
        public string tariff_code { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime? valid_to { get; set; }
    }

    public class Gas_Meter_Points
    {
        public string mprn { get; set; }
        public int consumption_standard { get; set; }
        public Meter[] meters { get; set; }
        public Agreement[] agreements { get; set; }
    }



   

    



}
