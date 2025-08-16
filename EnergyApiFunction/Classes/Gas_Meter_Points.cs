using System;
using   Energy_Consumption_Function.Interfaces;

namespace Energy_Consumption_Function.Classes
{
    public class Gas_Meter_Points : IMeterPoint
    {
        public string mprn { get; set; }
        public int consumption_standard { get; set; }
        public Meter[] meters { get; set; }
        public Agreement[] agreements { get; set; }
    }
}
