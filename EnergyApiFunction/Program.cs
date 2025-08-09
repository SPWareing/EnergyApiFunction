using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Energy_Consumption_Function.Classes;
using System.Net.Http;
using System;

namespace Energy_Consumption_Function
{
    public class Program
    {
        static void Main(string[] args)
        {
          

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();                  
                    services.AddHttpClient("Octopus", client =>
                    {
                        client.BaseAddress = new Uri("https://api.octopus.energy/v1/");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        string userName = Environment.GetEnvironmentVariable("ApiKEY");
                        string password = "";
                        var byteArray = System.Text.Encoding.ASCII.GetBytes($"{userName}:{password}");
                        var base64Credentials = Convert.ToBase64String(byteArray);
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials);
                    });

                })
                .Build();
            host.Run();
        }
    }
}
