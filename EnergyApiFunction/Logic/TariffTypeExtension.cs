using System;
using Energy_Consumption_Function.Enums;

namespace Energy_Consumption_Function.Logic
{
    public static class TariffTypeExtension
    {
        public static string ToFriendlyString(this TariffType tariffType)
        {
            return tariffType switch
            {
                TariffType.Electricity => "electricity-tariffs",
                TariffType.Gas => "gas-tariffs",
                _ => throw new ArgumentOutOfRangeException(nameof(tariffType), tariffType, null)
            };
        }
    }
}
