using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MS_Learn_Sample_Function.Classes;
using MS_Learn_Sample_Function.Logic;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MS_Learn_Sample_Function
{
    public class OctopusConsumptionFunction
    {
        private readonly ILogger<OctopusConsumptionFunction> _logger;
        private readonly HttpClient _client;
        private readonly TodoClient _todoClient;
        

        public OctopusConsumptionFunction(ILogger<OctopusConsumptionFunction> logger, HttpClient client, TodoClient todoClient)
        {
            _logger = logger;
           _client = client;
            _todoClient = todoClient;
        }

        [Function("ConsumptionFunction")]

        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)


        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var common = new Common();

            var get = await _todoClient.GetTodo();

            string dateFrom = req.Query["from"];
            string dateTo = req.Query["to"];
            string energyType = req.Query["energyType"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            Energy energyResponse = JsonConvert.DeserializeObject<Energy>(requestBody);

            _logger.LogInformation($"From : {energyResponse}, To: {energyResponse?.to}");

            dateFrom = dateFrom ?? energyResponse?.from.ToString();
            dateTo = dateTo ?? energyResponse?.to.ToString();
            energyType = energyType ?? energyResponse?.energyType;
            _logger.LogInformation($"Logging Statement: {data}");


            if (string.IsNullOrEmpty(dateFrom) || string.IsNullOrEmpty(dateTo) || string.IsNullOrEmpty(energyType))
            {

                var responseMessageData = req.CreateResponse(HttpStatusCode.BadRequest);
                responseMessageData.Headers.Add("Content-Type", "application/json");
                responseMessageData.WriteString("Please pass a date range and energy type on the query string or in the request body");
                return responseMessageData;
            }




            try
            {
                _logger.LogInformation("Calling GetEnergyConsumption: From: {0}, {1}", dateFrom, dateTo);
                var response = await common.GetEnergyConsumption(dateFrom, dateTo, energyType, _logger);
                _logger.LogInformation("Calling GetGasConsumption: From: {0}, {1}", dateFrom, dateTo);
                var gasResponse = await common.GetGasConsumption(dateFrom, dateTo, _logger);



                var mergedResponse = new { Electricity = response, Gas = gasResponse };


                _logger.LogInformation("Response: {0}", JsonConvert.SerializeObject(mergedResponse));

                string responseMessage = JsonConvert.SerializeObject(mergedResponse);

                var responseMessageData = req.CreateResponse(HttpStatusCode.OK);
                responseMessageData.Headers.Add("Content-Type", "application/json");
                responseMessageData.WriteString(responseMessage);
                return responseMessageData;
                //

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                var responseMessageData = req.CreateResponse(HttpStatusCode.BadRequest);
                responseMessageData.Headers.Add("Content-Type", "application/json");
                responseMessageData.WriteString("An error occurred");
                return responseMessageData;
            }






        }



    }



}
