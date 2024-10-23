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
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var common = new Common();



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
                return FormatResponse(req, HttpStatusCode.BadRequest, "Please pass a date range and energy type on the query string or in the request body");
            }




            try
            {
                var account = await common.GetAccountDetails(_logger);

                var dateFromUtc = DateTime.Parse(dateFrom).Date;
                var dateToUtc = DateTime.Parse(dateTo).Date;

                var elecTariff = account.properties.First()
                    .electricity_meter_points.First()
                    .agreements
                    .Where(x => dateFromUtc  >=  x.valid_from.Date && (dateToUtc <= x.valid_to || x.valid_to is null))
                    .ToList();

                var gasTariff = account.properties.First()
                    .gas_meter_points.First()
                    .agreements
                    .Where(x => dateFromUtc  >=  x.valid_from.Date && (dateToUtc <= x.valid_to || x.valid_to is null))
                    .ToList();


                var tariff = await common.GetTariff<ElecTariff>(dateFrom, dateTo, gasTariff.FirstOrDefault().tariff_code, "electricity-tariffs", _logger);

                var tariffGas = await common.GetTariff<GasTariff>(dateFrom, dateTo, gasTariff.FirstOrDefault().tariff_code,  "gas-tariffs", _logger);

                var dd = tariff.results.Where(x => x.payment_method == "DIRECT_DEBIT").FirstOrDefault();    

                var ddGas = tariffGas.results.Where(x => x.payment_method == "DIRECT_DEBIT").FirstOrDefault();

                _logger.LogInformation("Calling GetEnergyConsumption: From: {0}, {1}", dateFrom, dateTo);
                var response = await common.GetEnergyConsumption(dateFrom, dateTo, _logger);

                var join = response.results.Where(x => x.interval_start.Date >= dd.valid_from.Date)
                    .Select(x => new
                    {x.interval_start, x.consumption, x.interval_end, dd.value_exc_vat, dd.value_inc_vat
                    }).ToList();

                _logger.LogInformation("Calling GetGasConsumption: From: {0}, {1}", dateFrom, dateTo);
                var gasResponse = await common.GetGasConsumption(dateFrom, dateTo, _logger);

                var gasJoin = gasResponse.results.Where(x => x.interval_start.Date >= ddGas.valid_from.Date)
                    .Select(x => new
                    {x.interval_start, x.consumption, x.interval_end, ddGas.value_exc_vat, ddGas.value_inc_vat
                                       }).ToList();

                var mergedResponse = new { Electricity = join, Gas =gasJoin };
                _logger.LogInformation("Response: {0}", JsonConvert.SerializeObject(mergedResponse));
                string responseMessage = JsonConvert.SerializeObject(mergedResponse);
                return FormatResponse(req, HttpStatusCode.OK, responseMessage);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return FormatResponse(req, HttpStatusCode.BadRequest, "An error occured");
            }
        }

        public static HttpResponseData FormatResponse(HttpRequestData req, HttpStatusCode HttpStatusCode, string message)
        {
           var responseMessageData = req.CreateResponse(HttpStatusCode);
           responseMessageData.Headers.Add("Content-Type", "application/json");
           responseMessageData.WriteString(message);
           return responseMessageData;
        }



    }



}
