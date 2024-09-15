using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MS_Learn_Sample_Function.Classes
{
    public record class Energy(

        DateTime? from = null,
        DateTime? to = null,
        string energyType = null
        );
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
