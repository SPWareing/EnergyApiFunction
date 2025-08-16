using System;
using Energy_Consumption_Function.Interfaces;

namespace Energy_Consumption_Function.Classes
{
    public class Electricity_Meter_Points: IMeterPoint
{
    public string mpan { get; set; }
    public int profile_class { get; set; }
    public int consumption_standard { get; set; }
    public Meter[] meters { get; set; }
    public Agreement[] agreements { get; set; }
    public bool is_export { get; set; }
}
}
