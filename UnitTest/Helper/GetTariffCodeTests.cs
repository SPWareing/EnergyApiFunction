using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using UnitTest.TestBase;
using System.Runtime.InteropServices.Swift;
using Energy_Consumption_Function.Logic;

namespace UnitTest.Helper
{
    [TestClass]
    [TestCategory("HelperFunctions")]
    public class GetTariffCodeTests : TestBaseClass
    {
        [TestMethod]
        [DataRow("ABC-12-34-56", "ABC-12-34-56")] // valid
        [DataRow("1234/ABC-12-34-56", "ABC-12-34-56")] // valid
        [DataRow("XYZ-99-88-77", "XYZ-99-88-77")] // valid
        [DataRow("INVALID", "")]                  // invalid
        [DataRow("", "")]                         // empty
        public void GetTariffCode_ShouldReturnValidTariffCode(string input, string expected)
        {
            // Arrange
            Func<object, Type, bool> state = (v, t) => v?.ToString()?.CompareTo("Invalid Tariff Code") == 0;

            // Act
            var result = HelperFunctions.GetTariffCode(input, LoggerMock.Object);

            // Assert
            Assert.AreEqual(expected, result);
            var times = string.IsNullOrEmpty(expected)? Times.Once() : Times.Never();
           
            VerifyLogger(
                LogLevel.Error,
                "Invalid Tariff Code",
                times
            );

        }
    }
}
