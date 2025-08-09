using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Energy_Consumption_Function.Logic;

namespace UnitTest.Helper
{
    [TestClass]
   public class GetDatesTests
    {
        [TestMethod]
        [TestCategory("HelperFunctions")]
        public void GetDates_ShouldReturnCorrectDates()
        {
            // Arrange
            string dateFrom = "2023-01-01";
            string dateTo = "2023-01-31";

            string dateFromExpected = "2023-01-01T00:00:00Z";
            string dateToExpected = "2023-01-31T00:00:00Z";
            // Act
            var obj = HelperFunctions.GetDates(dateFrom, dateTo);
            // Assert
            Assert.AreEqual(dateFromExpected, obj["period_from"]);
            Assert.AreEqual(dateToExpected, obj["period_to"]);
        }
    }
}
