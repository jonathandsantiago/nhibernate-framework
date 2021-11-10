using Framework.Helper.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Helper.Extension
{
    public static class EnumExtension
    {
        public static bool HasAttribute<TAttr>(this Enum value) where TAttr : Attribute
        {
            return EnumHelper.GetAttribute<TAttr>(value) != null;
        }

        public static bool HasFlagIn(this Enum value, params Enum[] flags)
        {
            foreach (Enum item in flags)
            {
                if (value.HasFlag(item))
                {
                    return true;
                }
            }

            return false;
        }

        public static int ToInt(this object enumValue)
        {
            return (int)enumValue;
        }

        public static string GetLabelValorBitwise<T>(this Enum value, string separatorChar = null)
        {
            return EnumHelper.GetLabelBitwise<T>(Convert.ToInt32(value), separatorChar);
        }

        public static string GetLabelValorBitwise<T>(this Enum value, int intValue, string separatorChar = null)
        {
            return EnumHelper.GetLabelBitwise<T>(intValue, separatorChar);
        }

        public static string ToIntString(this Enum value)
        {
            return Convert.ToInt32(value).ToString();
        }

        public static string ToDisplayNameString(this Enum value)
        {
            return EnumHelper.GetDisplayName(value);
        }

        public static TResult Last<TResult>(this Enum value)
        {
            List<TResult> values = Enum.GetValues(typeof(TResult)).Cast<TResult>().ToList();
            return values.LastOrDefault();
        }

        public static IList<Enum> GetFlags(this Enum value)
        {
            return Enum.GetValues(value.GetType()).Cast<Enum>().Where(value.HasFlag).ToList();
        }
    }
}