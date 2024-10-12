using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MS_Learn_Sample_Function.Classes;
using System.Net.Http;

namespace MS_Learn_Sample_Function
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
