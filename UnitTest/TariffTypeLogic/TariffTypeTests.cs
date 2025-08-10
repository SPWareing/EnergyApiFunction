using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.TestBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Energy_Consumption_Function.Enums;
using Energy_Consumption_Function.Logic;

namespace UnitTest.TariffTypeLogic
{
    [TestClass]
    [TestCategory("TariffType")]
    public class TariffTypeTests :TestBaseClass
    {
        [TestMethod]
        [DataRow(TariffType.Electricity, "electricity-tariffs")]
        [DataRow(TariffType.Gas, "gas-tariffs")]
        public void ToFriendlyString_ShouldReturnCorrectString(TariffType tariffType, string expected)
        {
            // Act
            var result = tariffType.ToFriendlyString();
            // Assert
            Assert.AreEqual(expected, result);
        }
        [TestMethod]       
        [DataRow((TariffType)999)] // Invalid TariffType
        public void ToFriendlyString_ShouldThrowArgumentOutOfRangeException(TariffType tariffType)
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tariffType.ToFriendlyString());
        }
    }
}
