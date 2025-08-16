using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders;
using UnitTest.TestBase;
using Moq;
using Moq.Protected;
using Energy_Consumption_Function.Classes;
using Energy_Consumption_Function.Logic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
namespace UnitTest.Common
{
    [TestClass]
    [TestCategory("CommonFunctions")]
    public class GetResultAsyncTests: TestBaseClass
    {
        [TestCleanup]
        public void Cleanup()
        {
            HttpClientMock.Reset();
            LoggerMock.Reset();
        }

        [TestMethod]
        public async Task ShouldReturnResultAsync()
        {
            var ct = 5;

            List<Result> results = new List<Result>();

            for (var i = 0; i < ct; i++)
            {
                results.Add(new ResultsBuilder()
                    .WithConsumption(i * 10.0f)
                    .WithIntervalStart(DateTime.UtcNow.AddDays(-i))
                    .WithIntervalEnd(DateTime.UtcNow.AddHours(-i + 1))
                    .Build());
            }

            var consumption = new ConsumptionBuilder()
                .WithResults(results.ToArray()).WithCount(ct)
                .Build();

            // Arrange
            var accountDetails = new AccountDetailsBuilder().Build();
            var dateFrom = "2023-01-01";
            var dateTo = "2023-01-31";
            
            
            HandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(consumption))
                });

            var client = new HttpClientBuilder()
                .WithHttpMessageHandler(HandlerMock.Object)
                .Build();
            var common = new  Energy_Consumption_Function.Logic.Common(client, LoggerMock.Object);
            // Act
            var result = await common.GetResultAsync<Consumption>($"accounts/{accountDetails.number}", dateFrom, dateTo);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(consumption.count, result.count);

      

            for (int i = 0; i < consumption.results.Length; i++)
            {
                Assert.AreEqual(consumption.results[i].consumption, result.results[i].consumption);
                Assert.AreEqual(consumption.results[i].interval_start, result.results[i].interval_start);
                Assert.AreEqual(consumption.results[i].interval_end, result.results[i].interval_end);
            }
        }
    }
}
