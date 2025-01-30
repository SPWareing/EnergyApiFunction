using Energy_Consumption_Function.Classes;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Energy_Consumption_Function.Logic
{
    public  class HelperFunctions
    {
        /// <summary>
        /// Returns a dictionary of the dates in UTC format.
        /// </summary>
        /// <param name="dateFrom">Start date.</param>
        /// <param name="dateTo">End date.</param>
        /// <returns>A dictionary containing the start and end dates in UTC format.</returns>
        public static Dictionary<string, string> GetDates(string dateFrom, string dateTo)
        {
            string dateFormat = "yyyy-MM-ddTHH:mm:ssZ";
            return new Dictionary<string, string>()
            {
                { "period_from",DateTime.Parse(dateFrom).ToString(dateFormat) },
                { "period_to" ,DateTime.Parse(dateTo).ToString(dateFormat) },
                { "order_by" ,"period"},
                {"page_size","200"}
            };

        }

        /// <summary>
        /// Regex to extract the Tariff Code from the string.
        /// </summary>
        /// <param name="tariffCode"> Variation of the base tariff </param>
        /// <param name="log"> ILogger </param>
        /// <returns>String</returns>

        public static string GetTariffCode(string tariffCode, ILogger  _log)
        {
            var rgx = new Regex(@"[A-Z]{3}-\d{2}-\d{2}-\d{2}");

            if (rgx.IsMatch(tariffCode))
            {
                return rgx.Matches(tariffCode)[0].Value;
            }
            else
            {
                _log.LogError("Invalid Tariff Code");
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns a List of Agreements for all the Tariffs in date range.
        /// </summary>
        /// <param name="account"> Account Details of the User </param>
        /// <param name="dateFrom"> UTC Format Date </param>
        /// <param name="dateTo"> UTC Format Date </param>
        /// <param name="tariffType"> Either "electricity-tariffs" or "gas-tariffs" </param>
        /// <returns> A list of <see cref="Agreement"/></returns>
        public static List<Agreement> GetAgreementCost(AccountDetails account, DateTime dateFrom, DateTime dateTo, string tariffType)
        {
            switch (tariffType)
            {

                case "electricity-tariffs":
                    return account.properties.First()
                                .electricity_meter_points.First()
                                .agreements
                                .Where(x => dateFrom >= x.valid_from.Date && (dateTo <= x.valid_to || x.valid_to is null))
                                .ToList();
                case "gas-tariffs":
                    return account.properties.First()
                                .gas_meter_points.First()
                                .agreements
                                .Where(x => dateFrom >= x.valid_from.Date && (dateTo <= x.valid_to || x.valid_to is null))
                                .ToList();
                default:
                    return new List<Agreement>();
            }


        }

        public static List<MergedResponse> JoinConsumptionTariffs(Consumption response, ResultTariff dd)
             {
           var join = response.results.Where(x => x.interval_start.Date >= dd.valid_from.Date)
                    .Select(x => new MergedResponse
                    {
                        interval_start = x.interval_start,
                        interval_end = x.interval_end,
                        consumption = x.consumption,
                        value_exc_vat = dd.value_exc_vat,
                        value_inc_vat = dd.value_inc_vat
                    }).ToList();

            return join;
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




