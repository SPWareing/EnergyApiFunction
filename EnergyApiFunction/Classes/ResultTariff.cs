using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class ResultTariff
    {
        public float value_exc_vat { get; set; }
        public float value_inc_vat { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime? valid_to { get; set; }
        public string payment_method { get; set; }
    }
}
