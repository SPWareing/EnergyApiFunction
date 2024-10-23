using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Energy_Consumption_Function.Classes;
using System.Net.Http;

namespace Energy_Consumption_Function
{
    public class Program
    {
        static void Main(string[] args)
        {
            FunctionsDebugger.Enable();

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    services.AddApplicationInsightsTelemetryWorkerService();
                    services.ConfigureFunctionsApplicationInsights();
                    services.AddSingleton<HttpClient>();
                    services.AddSingleton<TodoClient>();
                })
                .Build();
            host.Run();
        }
    }
}
