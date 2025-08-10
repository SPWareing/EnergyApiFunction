using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders.Base;
using Energy_Consumption_Function.Classes;

namespace UnitTest.Builders
{
    public class ResultsBuilder: BuilderBaseClass<Result>
    {
        public ResultsBuilder()
        {
            _class = new Result
            {
                consumption = 0.0f,
                interval_start = DateTime.MinValue,
                interval_end = DateTime.MinValue
            };
        }
        public ResultsBuilder WithConsumption(float consumption)
        {
            _class.consumption = consumption;
            return this;
        }
        public ResultsBuilder WithIntervalStart(DateTime intervalStart)
        {
            _class.interval_start = intervalStart;
            return this;
        }
        public ResultsBuilder WithIntervalEnd(DateTime intervalEnd)
        {
            _class.interval_end = intervalEnd;
            return this;
        }
    }
}
