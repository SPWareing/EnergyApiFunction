using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Energy_Consumption_Function.Classes
{
    public class Electricity_Meter_Points
{
    public string mpan { get; set; }
    public int profile_class { get; set; }
    public int consumption_standard { get; set; }
    public Meter[] meters { get; set; }
    public Agreement[] agreements { get; set; }
    public bool is_export { get; set; }
}
}
