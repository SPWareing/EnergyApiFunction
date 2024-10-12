using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MS_Learn_Sample_Function.Classes
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

}
