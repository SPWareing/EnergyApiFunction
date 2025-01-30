using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class ElecTariff
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public ResultTariff[] results { get; set; }
    }
}
