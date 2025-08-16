using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders.Base;
using Energy_Consumption_Function.Classes;
using Microsoft.Identity.Client;

namespace UnitTest.Builders
{
    public class TariffListBuilder : BuilderBaseClass<TariffList>
    {
        public TariffListBuilder()
        {
            _class = new TariffList
            {
                value_exc_vat = 0.0f,
                value_inc_vat = 0.0f,
                valid_from = DateTime.MinValue,
                valid_to = null,
                payment_method = String.Empty

            };
        }

        public TariffListBuilder WithValueExcVat(float valueExcVat)
        {
            _class.value_exc_vat = valueExcVat;
            return this;
        }

        public TariffListBuilder WithValueIncVat(float valueIncVat)
        {
            _class.value_inc_vat = valueIncVat;
            return this;
        }

        public TariffListBuilder WithValidFrom(DateTime validFrom)
        {
            _class.valid_from = validFrom;
            return this;
        }

        public TariffListBuilder WithValidTo(DateTime? validTo)
        {
            _class.valid_to = validTo;
            return this;
        }

        public TariffListBuilder WithPaymentMethod(string paymentMethod)
        {
            _class.payment_method = paymentMethod;
            return this;
        }
    }
}

