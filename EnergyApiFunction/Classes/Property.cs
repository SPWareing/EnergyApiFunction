using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class Property
    {
        public int id { get; set; }
        public DateTime moved_in_at { get; set; }
        public object moved_out_at { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string address_line_3 { get; set; }
        public string town { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public Electricity_Meter_Points[] electricity_meter_points { get; set; }
        public Gas_Meter_Points[] gas_meter_points { get; set; }
    }
}
