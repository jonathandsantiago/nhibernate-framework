using Framework.Helper.Extension;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading;

namespace Framework.Helper.Helpers
{
    public static class DateTimeHelper
    {
        public static bool Different(DateTime? date, DateTime? dateComparison)
        {
            return Nullable.Compare(date, dateComparison) != 0;
        }

        public static bool IsBigger(DateTime? date, DateTime? dateComparison)
        {
            return Nullable.Compare(date, dateComparison) > 0;
        }

        public static bool IsGreaterOrEqual(DateTime? date, DateTime? dateComparison)
        {
            return Nullable.Compare(date, dateComparison) >= 0;
        }

        public static bool IsNullOrEmpty(DateTime? date, bool validateMinimumDate = true)
        {
            if (date == null || (validateMinimumDate && date < SqlDateTime.MinValue.Value))
            {
                return true;
            }

            return false;
        }

        public static bool Minimum(DateTime date)
        {
            return date <= SqlDateTime.MinValue.Value;
        }

        public static int DiffTotalDays(DateTime dateA, DateTime dateB)
        {
            return Convert.ToInt32((dateA.Date - dateB.Date).TotalDays);
        }

        public static int DiffTotalDays(DateTime dateA, DateTime dateB, int min)
        {
            int result = DiffTotalDays(dateA, dateB);
            return result > min ? result : min;
        }

        public static int? DiffTotalDays(DateTime? dataA, DateTime? dataB)
        {
            if (!dataA.HasValue || !dataB.HasValue)
            {
                return new int?();
            }

            return Convert.ToInt32((dataA.Value.Date - dataB.Value.Date).TotalDays);
        }

        public static int DiffTotalMonths(this DateTime start, DateTime end)
        {
            return (start.Year * 12 + start.Month) - (end.Year * 12 + end.Month);
        }

        public static int DiffTotalHours(DateTime dateA, DateTime dateB)
        {
            return Convert.ToInt32((dateA - dateB).TotalHours);
        }

        public static string DayWeekByExtended(DateTime date)
        {
            return DayWeekByExtended(date.DayOfWeek);
        }

        public static string DayWeekByExtended(DayOfWeek dateWeek)
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetDayName(dateWeek);
        }

        public static string FormatDateTime(DateTime date)
        {
            return date.ToString("dd/MM/yyy HH:mm");
        }

        public static string MonthByExtension(DateTime date)
        {
            return MonthByExtension(date.Month);
        }

        public static string MonthByExtension(int month)
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }

        public static DateTime AddMinutesRange(DateTime date, int interval = 30)
        {
            if (interval > 60 || interval < 1)
            {
                throw new System.Exception("O parâmetro \"intervalo\" deve estar entre 1 e 60.");
            }

            int minuto = date.Minute;

            while (minuto % interval != 0 && minuto < 60)
            {
                minuto++;
            }

            return date.AddMinutes(minuto - date.Minute);
        }

        public static DateTime Bigger(params DateTime[] dates)
        {
            return dates.Max();
        }

        public static DateTime? Bigger(params DateTime?[] dates)
        {
            DateTime[] datesNotNull = dates.Where(c => c.HasValue).SelectArray(c => c.Value);

            return datesNotNull.Count() > 0 ? Bigger(datesNotNull) : new DateTime?();
        }

        public static DateTime DateOnly(DateTime date)
        {
            return date.Date;
        }

        public static DateTime? GetBiggerDate(DateTime? dateA, DateTime? dateB)
        {
            return IsBigger(dateA, dateB) ? dateA : dateB;
        }

        public static DateTime GenerateData(DateTime date, TimeSpan time)
        {
            return new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);
        }

        public static DateTime GenerateData(int year, int month, int day)
        {
            return new DateTime(year, month, day <= DateTime.DaysInMonth(year, month) ? day : DateTime.DaysInMonth(year, month));
        }

        public static DateTime GetEasterDate(int year)
        {
            int c = year / 100;
            int n = year - 19 * (year / 19);
            int k = (c - 17) / 25;
            int i = c - c / 4 - (c - k) / 3 + 19 * n + 15;
            i = i - 30 * (i / 30);
            i = i - (i / 28) * (1 - (1 / 28) * (29 / (i + 1)) * ((21 - n) / 11));
            int j = year + year / 4 + i + 2 - c + c / 4;
            j = j - 7 * (j / 7);
            int l = i - j;
            int month = 3 + (l + 40) / 44;
            int day = l + 28 - 31 * (month / 4);

            return new DateTime(year, month, day);
        }
        
        public static DateTime LastDateMonth(DateTime date)
        {
            int ultimoDia = DateTime.DaysInMonth(date.Year, date.Month);
            return new DateTime(date.Year, date.Month, ultimoDia);
        }

        public static DateTime LastDateYear(DateTime date)
        {
            int ultimoDia = DateTime.DaysInMonth(date.Year, 12);
            return new DateTime(date.Year, 12, ultimoDia);
        }

        public static DateTime NextDay(DateTime dataBase, int diaRequerido)
        {
            int ano = diaRequerido <= dataBase.Day ? dataBase.AddMonths(1).Year : dataBase.Year;
            int mes = diaRequerido <= dataBase.Day ? dataBase.AddMonths(1).Month : dataBase.Month;
            int dia = diaRequerido > DateTime.DaysInMonth(ano, mes) ? DateTime.DaysInMonth(ano, mes) : diaRequerido;

            return new DateTime(ano, mes, dia);
        }

        public static DateTime Smaller(params DateTime[] dates)
        {
            return dates.Min();
        }

        public static DateTime TruncateSeconds(DateTime date)
        {
            return date.AddSeconds(date.Second * -1);
        }

        public static IList<DateTime> GetDates(DateTime begin, DateTime end)
        {
            IList<DateTime> dates = new List<DateTime>();

            for (DateTime i = begin; i <= end; i = i.AddDays(1))
            {
                dates.Add(i);
            }

            return dates;
        }
    }
}