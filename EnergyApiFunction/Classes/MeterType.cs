using System;

namespace Energy_Consumption_Function.Classes
{
    public record  MeterType
    {
        public string Type { get; init; }

        public string Mpan { get; init; }

        public string Serial { get; init; } 
    }
}
