using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders.Base;
using Energy_Consumption_Function.Classes;

namespace UnitTest.Builders
{
    public class TariffBuilder :  BuilderBaseClass<Tariff>
    {
        public TariffBuilder()
        {
            _class = new Tariff
            {
                count = 0,
                next = String.Empty,
                previous = String.Empty,
                results = new TariffList[0]
            };
        }
        public TariffBuilder WithCount(int count)
        {
            _class.count = count;
            return this;
        }
        public TariffBuilder WithNext(object next)
        {
            _class.next = next;
            return this;
        }
        public TariffBuilder WithPrevious(object previous)
        {
            _class.previous = previous;
            return this;
        }
        public TariffBuilder WithResults(TariffList[] results)
        {
            _class.results = results;
            return this;
        }
    }
}
