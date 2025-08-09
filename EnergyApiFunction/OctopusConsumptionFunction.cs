using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Energy_Consumption_Function.Classes;
using Energy_Consumption_Function.Logic;
using Energy_Consumption_Function.Enums;
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

        public OctopusConsumptionFunction(ILogger<OctopusConsumptionFunction> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient("Octopus");
            ;
        }

        [Function("ConsumptionFunction")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            _logger.LogInformation($"{nameof(OctopusConsumptionFunction)} processed a request.");
           

            string dateFrom = req.Query["from"];
            string dateTo = req.Query["to"];
            string energyType = req.Query["energyType"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            Energy energyResponse = JsonConvert.DeserializeObject<Energy>(requestBody);

            _logger.LogInformation($"From : {energyResponse}, To: {energyResponse?.to}");

            dateFrom ??= energyResponse?.from.ToString();
            dateTo ??= energyResponse?.to.ToString();
            energyType ??= energyResponse?.energyType;
            _logger.LogInformation($"Logging Statement: {data}");


            if (string.IsNullOrEmpty(dateFrom) || string.IsNullOrEmpty(dateTo) || string.IsNullOrEmpty(energyType))
            {
                return HelperFunctions.FormatResponse(req, HttpStatusCode.BadRequest, "Please pass a date range and energy type on the query string or in the request body");
            }
            
            var common = new Common(_client, _logger);

            try
            {
                var account = await common.GetAccountDetails();

                var dateFromUtc = DateTime.Parse(dateFrom).Date;
                var dateToUtc = DateTime.Parse(dateTo).Date;

                var elecTariff = HelperFunctions.GetAgreementCost(account, dateFromUtc, dateToUtc, TariffType.Electricity);

                var gasTariff = HelperFunctions.GetAgreementCost(account, dateFromUtc, dateToUtc, TariffType.Gas);

                var tariff = await common.GetTariff(dateFrom, dateTo, elecTariff.FirstOrDefault().tariff_code,TariffType.Electricity);

                var tariffGas = await common.GetTariff(dateFrom, dateTo, gasTariff.FirstOrDefault().tariff_code, TariffType.Gas);

                var dd = HelperFunctions.GetFirstTariffResponse(tariff);

                var ddGas = HelperFunctions.GetFirstTariffResponse(tariffGas);

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
                string responseMessage = JsonConvert.SerializeObject(mergedResponse);
                _logger.LogInformation("Electricity Response: {0}, Gas Response: {1}", mergedResponse.Electricity.Count, mergedResponse.Gas.Count);


                return HelperFunctions.FormatResponse(req, HttpStatusCode.OK, responseMessage);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return HelperFunctions.FormatResponse(req, HttpStatusCode.BadRequest, "An error occured");
            }
            finally {
                // _client.Dispose();
                
            }
        }

       
    }



}
