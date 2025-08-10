using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Energy_Consumption_Function.Classes;
using UnitTest.Builders.Base;

namespace UnitTest.Builders
{
    public class ConsumptionBuilder : BuilderBaseClass<Consumption>
    {
         public ConsumptionBuilder()
        {
            _class = new Consumption
            {
                count = 0,
                next = String.Empty,
                previous = String.Empty,
                results = Array.Empty<Result>()
            };
        }
        public ConsumptionBuilder WithCount(int count)
        {
            _class.count = count;
            return this;
        }
        public ConsumptionBuilder WithNext(string next)
        {
            _class.next = next;
            return this;
        }
        public ConsumptionBuilder WithPrevious(object previous)
        {
            _class.previous = previous;
            return this;
        }
        public ConsumptionBuilder WithResults(Result[] results)
        {
            _class.results = results;
            return this;
        }
    }
}
