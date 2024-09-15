using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MS_Learn_Sample_Function.Classes;
using MS_Learn_Sample_Function.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace MS_Learn_Sample_Function
{
    public static class OctopusConsumptionFunction
    {
        

        [FunctionName("ConsumptionFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get",  Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var services = new ServiceCollection();
            services.AddSingleton<EnergyConsumptionClient>();
            services.BuildServiceProvider();
            var common = new Common();
            var client = new HttpClient();
            

            string dateFrom = req.Query["from"];
            string  dateTo = req.Query["to"];
            string energyType = req.Query["energyType"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();            
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            Energy energyResponse = JsonConvert.DeserializeObject<Energy>(requestBody);

            log.LogInformation($"From : {energyResponse?.from}, To: {energyResponse?.to}");

            dateFrom = dateFrom ?? energyResponse?.from.ToString();
           dateTo = dateTo ?? energyResponse?.to.ToString();
            energyType = energyType ?? energyResponse?.energyType;
            log.LogInformation($"Logging Statement: {data}");

            
            if (string.IsNullOrEmpty(dateFrom) || string.IsNullOrEmpty(dateTo) || string.IsNullOrEmpty(energyType))
            {
                return new BadRequestObjectResult("Please pass a date range and energy type on the query string or in the request body");
            }
          

            

            try
            { 
             var response = await common.GetEnergyConsumption(client, dateFrom, dateTo, energyType, log);
              string responseMessage = JsonConvert.SerializeObject(response);
              return new OkObjectResult(responseMessage);
            }catch(Exception ex)
            {
                log.LogError(ex.Message);
                return new BadRequestObjectResult("An error occurred");
            }
           

           
            

            
        }

        

    }


    
}
