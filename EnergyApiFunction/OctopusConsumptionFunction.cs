using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Energy_Consumption_Function.Classes;
using Energy_Consumption_Function.Logic;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Energy_Consumption_Function
{
    public class OctopusConsumptionFunction
    {
        private readonly ILogger<OctopusConsumptionFunction> _logger;
        private readonly HttpClient _client;

        public OctopusConsumptionFunction(ILogger<OctopusConsumptionFunction> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        [Function("ConsumptionFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(OctopusConsumptionFunction)} processed a request.");
            var common = new Common(_client, _logger);

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
                return HelperFunctions.FormatResponse(req, HttpStatusCode.BadRequest, "Please pass a date range and energy type on the query string or in the request body");
            }

            try
            {
                var account = await common.GetAccountDetails();

                var dateFromUtc = DateTime.Parse(dateFrom).Date;
                var dateToUtc = DateTime.Parse(dateTo).Date;

                var elecTariff = HelperFunctions.GetAgreementCost(account, dateFromUtc, dateToUtc, "electricity-tariffs");                  

                var gasTariff =  HelperFunctions.GetAgreementCost(account, dateFromUtc, dateToUtc, "gas-tariffs");

                var tariff = await common.GetTariff<ElecTariff>(dateFrom, dateTo, elecTariff.FirstOrDefault().tariff_code, "electricity-tariffs");

                var tariffGas = await common.GetTariff<GasTariff>(dateFrom, dateTo, gasTariff.FirstOrDefault().tariff_code,  "gas-tariffs");

                var dd = tariff.results.Where(x => x.payment_method == "DIRECT_DEBIT").FirstOrDefault();    

                var ddGas = tariffGas.results.Where(x => x.payment_method == "DIRECT_DEBIT").FirstOrDefault();

                _logger.LogInformation("Calling GetEnergyConsumption: From: {0}, {1}", dateFrom, dateTo);
                var response = await common.GetEnergyConsumption(dateFrom, dateTo);

                var join = HelperFunctions.JoinConsumptionTariffs(response, dd);

                _logger.LogInformation("Calling GetGasConsumption: From: {0}, {1}", dateFrom, dateTo);
                var gasResponse = await common.GetGasConsumption(dateFrom, dateTo);

                var gasJoin = HelperFunctions.JoinConsumptionTariffs(gasResponse, ddGas);              

                var mergedResponse = new RequestResponse
                {
                    Electricity = join,
                    Gas = gasJoin
                };  
                _logger.LogInformation("Response: {0}", JsonConvert.SerializeObject(mergedResponse));
                string responseMessage = JsonConvert.SerializeObject(mergedResponse);
                return HelperFunctions.FormatResponse(req, HttpStatusCode.OK, responseMessage);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return HelperFunctions.FormatResponse(req, HttpStatusCode.BadRequest, "An error occured");
            }
        }

       
    }



}
