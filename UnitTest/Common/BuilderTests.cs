using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energy_Consumption_Function.Classes;
using UnitTest.Builders;

namespace UnitTest.Common
{
    [TestClass]
    [TestCategory("BuilderTests")]
    public class BuilderTests
    {
        [TestMethod]
        public void ResultsBuilder_ShouldCreateResultWithGivenValues()
        {
            // Arrange
            var expectedConsumption = 123.45f;
            var expectedStart = new DateTime(2023, 1, 1, 0, 0, 0);
            var expectedEnd = new DateTime(2023, 1, 1, 1, 0, 0);
            // Act
            var result = new ResultsBuilder()
                .WithConsumption(expectedConsumption)
                .WithIntervalStart(expectedStart)
                .WithIntervalEnd(expectedEnd)
                .Build();
            // Assert
            Assert.AreEqual(expectedConsumption, result.consumption);
            Assert.AreEqual(expectedStart, result.interval_start);
            Assert.AreEqual(expectedEnd, result.interval_end);
        }
        [TestMethod]
        public void ResultsBuilder_ShouldCreateResultsList()
        {
            // Arrange
            var dateLoop = new DateTime(2023, 1, 1, 0, 0, 0);
            // Act
            Result[] resultList = [..Enumerable.Range(0, 5)
                                                     .Select(i => new ResultsBuilder()
                                                         .WithConsumption(i * 10.0f)
                                                         .WithIntervalStart(dateLoop.AddDays(i))
                                                         .WithIntervalEnd(dateLoop.AddDays(i + 1))
                                                         .Build())];
         

            // Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(5, resultList.Length);
            for (int i = 0; i < resultList.Length; i++)
            {
                Assert.AreEqual(i * 10.0f, resultList[i].consumption);
                Assert.AreEqual(dateLoop.AddDays(i), resultList[i].interval_start);
                Assert.AreEqual(dateLoop.AddDays(i + 1), resultList[i].interval_end);
            }
        }
    }

    
}
