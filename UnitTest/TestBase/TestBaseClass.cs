using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UnitTest.TestBase
{
    public abstract  class TestBaseClass
    {
        protected Mock<ILogger> LoggerMock { get; }
        protected Mock<HttpClient> HttpClientMock { get; } = new Mock<HttpClient>();
        protected Mock<HttpMessageHandler> HandlerMock { get; } = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        protected TestBaseClass()
        {
            LoggerMock = new Mock<ILogger>();
        }
        protected void VerifyLogger(
            LogLevel level,
            string expectedMessage,
            Times? times = null)
        {
            times ??= Times.Never();
            Func<object, Type, bool> state = (v, t) => v?.ToString()?.CompareTo(expectedMessage) == 0;
            LoggerMock.Verify(
                log => log.Log(
                    level,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => state(v,t)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                (Times)times);
        }


    }
}

