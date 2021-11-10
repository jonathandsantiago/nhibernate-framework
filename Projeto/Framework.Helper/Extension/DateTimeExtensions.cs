using Framework.Helper.Helpers;
using System;
using System.Globalization;

namespace Framework.Helper.Extension
{
    public static class DateTimeExtensions
    {
        public static bool IsBirthday(this DateTime date, DateTime beginDate, DateTime endDate)
        {
            return string.Compare(date.ToString("MMdd"), beginDate.ToString("MMdd")) >= 0 &&
                string.Compare(date.ToString("MMdd"), endDate.ToString("MMdd")) <= 0;
        }

        public static bool IsCurrentDate(this DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }

        public static bool IsLessOrEqualCurrentDateTime(this DateTime date)
        {
            return date <= DateTime.Now;
        }

        public static int WeekOfYear(this DateTime date)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(date);

            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                date = date.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static bool Between(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }

        public static int GetAge(this DateTime? birthDay, DateTime currentDate)
        {
            if (DateTimeHelper.IsNullOrEmpty(birthDay))
            {
                return 0;
            }

            return birthDay.Value.GetAge(currentDate);
        }

        public static int GetAge(this DateTime birthDay, DateTime currentDate)
        {
            int age = currentDate.Year - birthDay.Year;

            if ((birthDay.Month > DateTime.Now.Month) || (birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day))
            {
                age--;
            }

            return age;
        }

        public static int GetAgeMonth(this DateTime birthDay, DateTime currentDate)
        {
            int age = currentDate.Month - birthDay.Month;

            if ((birthDay.Year <= DateTime.Now.Year && birthDay.Month <= DateTime.Now.Month && birthDay.Day > DateTime.Now.Day) || birthDay.Year == DateTime.Now.Year && birthDay.Month == DateTime.Now.Month && birthDay.Day > DateTime.Now.Day)
            {
                age--;
            }

            return age < 0 ? age + 12 : age;
        }

        public static DateTime TruncateSeconds(this DateTime dateTime)
        {
            return dateTime.AddSeconds(dateTime.Second * -1);
        }

        public static TimeSpan DiffTotalHours(this DateTime? dataA, DateTime? dataB)
        {
            if (!dataA.HasValue || !dataB.HasValue)
            {
                return TimeSpan.Zero;
            }

            return TimeSpan.FromDays((dataA.Value.Date - dataB.Value.Date).TotalHours);
        }

        public static int DiffTotalDays(this DateTime dataA, DateTime dataB)
        {
            return Convert.ToInt32((dataA.Date - dataB.Date).TotalDays);
        }

        public static DateTime? LastTimeOfDay(this DateTime? date)
        {
            return date.HasValue ? date.Value.Date.AddDays(1).AddSeconds(-1) : (DateTime?)null;
        }
    }
}
