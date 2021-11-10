using System;

namespace Framework.Helper.Extension
{
    public static class DecimalExtension
    {
        public static decimal Percent(this decimal value)
        {
            return value / 100;
        }

        public static decimal ToRoundDecimal(this decimal value, int floatingPoint = 2)
        {
            return decimal.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal IsNull(this decimal? value, decimal valueIsNull)
        {
            return value != null ? (decimal)value : valueIsNull;
        }

        public static bool Between(this decimal? value, decimal? start, decimal? end)
        {
            return value >= start && value <= end;
        }

        public static bool IsNullOrZero(this decimal? value)
        {
            return value == null || value == 0;
        }
    }
}
