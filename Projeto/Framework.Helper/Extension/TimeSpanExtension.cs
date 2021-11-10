using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Helper.Extension
{
    public static class TimeSpanExtension
    {
        public static long GetTruncateTotalDays(this TimeSpan timeSpan)
        {
            return Convert.ToInt64(Math.Truncate(timeSpan.TotalDays));
        }

        public static string FormatHourAndMinute(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"hh\:mm");
        }

        public static string FormatDayHourMinuteSecods(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(@"dd\.hh\:mm\:ss");
        }

        public static bool Between(this TimeSpan time, TimeSpan start, TimeSpan end)
        {
            return time >= start && time <= end;
        }

        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }
    }
}
