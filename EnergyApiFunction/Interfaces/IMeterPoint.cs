using Energy_Consumption_Function.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Interfaces
{
    public interface IMeterPoint
    {
        public int consumption_standard { get; set; }
        public Meter[] meters { get; set; }
        Agreement[] agreements { get; set; }
    }
}
