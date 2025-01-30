using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class MergedResponse
    {
        public DateTime interval_start { get; set; }
        public DateTime interval_end { get; set; }
        public float consumption { get; set; }
        public float value_exc_vat { get; set; }
        public float value_inc_vat { get; set; }


    }
}
