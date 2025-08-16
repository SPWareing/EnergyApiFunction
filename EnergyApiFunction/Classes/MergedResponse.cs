using System;
using Energy_Consumption_Function.Interfaces;

namespace Energy_Consumption_Function.Classes
{
    public class MergedResponse : IResult
    {
        public DateTime interval_start { get; set; }
        public DateTime interval_end { get; set; }
        public float consumption { get; set; }
        public float value_exc_vat { get; set; }
        public float value_inc_vat { get; set; }


    }
}
