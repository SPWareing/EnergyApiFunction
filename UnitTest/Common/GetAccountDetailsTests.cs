using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Energy_Consumption_Function.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTest.TestBase;
using UnitTest.Builders;
using Moq;
using Moq.Protected;
using System.Net;
namespace UnitTest.Common
{
    [TestClass]
    [TestCategory("CommonFunctions")]
    public class GetAccountDetailsTests : TestBaseClass

    {
        //private Mock<HttpMessageHandler> HandlerMock { get; } = new Mock<HttpMessageHandler>(MockBehavior.Strict)
        

        [TestCleanup]
        public void Cleanup()
        {
            HttpClientMock.Reset();
            LoggerMock.Reset();
            HandlerMock.Reset();
        }

        [TestMethod]
        public async Task ShouldReturnAccountDetails()
        {
            // Arrange
            var accountDetails = new AccountDetailsBuilder().Build();

            HandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(accountDetails))
                });

            var httpClient = new HttpClientBuilder()
                .WithHttpMessageHandler(HandlerMock.Object)
                .Build();


            var common = new Energy_Consumption_Function.Logic.Common(
                httpClient,
                LoggerMock.Object
            );

            // Act
            var result = await common.GetAccountDetails();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType<AccountDetails>(result);
            Assert.AreEqual(accountDetails.number, result.number);
        }

        [TestMethod]
        public async Task ShouldThrowExceptionOnError()
        {
            // Arrange
            HandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });
            var httpClient = new HttpClientBuilder()
                .WithHttpMessageHandler(HandlerMock.Object)
                .Build();
            var common = new Energy_Consumption_Function.Logic.Common(
                httpClient,
                LoggerMock.Object
            );
            // Act & Assert

            await Assert.ThrowsExceptionAsync<HttpRequestException>(() => common.GetAccountDetails());            
            //VerifyLogger(LogLevel.Error, "Error fetching account details: Response status code does not indicate success: 500 (Internal Server Error). ", Times.Once());
                

        }
    }
}
