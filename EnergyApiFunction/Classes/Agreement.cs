using System;

namespace Energy_Consumption_Function.Classes
{
    public class Agreement
    {
        public string tariff_code { get; set; }
        public DateTime valid_from { get; set; }
        public DateTime? valid_to { get; set; }
    }
}
