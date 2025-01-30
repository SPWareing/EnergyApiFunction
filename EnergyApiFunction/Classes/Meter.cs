using Energy_Consumption_Function.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class Meter
    {
        public string serial_number { get; set; }
        public Register[] registers { get; set; }
    }
}
