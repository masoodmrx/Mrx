namespace Mrx.Common.Extensions
{
    public static class DateTimeExtentions
    {
        //private static PersianCalendar pc = new PersianCalendar();
        //public static DateTime ToPersian(this DateTime dt)
        //    => new DateTime(pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
        //public static DateTime ToEnglish(this DateTime dt)
        //    => pc.ToDateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0);
        public static string ToShortDateString(this DateTime? dt, string def = "")
        {
            return dt.HasValue ? dt.Value.ToShortDateString() : def;
        }
        public static string ToLongDateString(this DateTime? dt, string def = "")
        {
            return dt.HasValue ? dt.Value.ToLongDateString() : def;
        }
        public static string ToShortTimeString(this DateTime? dt, string def = "")
        {
            return dt.HasValue ? dt.Value.ToShortTimeString() : def;
        }
        public static string ToLongTimeString(this DateTime? dt, string def = "")
        {
            return dt.HasValue ? dt.Value.ToLongTimeString() : def;
        }

        public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
        {
            return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
        }

        public static string GetTimeAgo(this DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
            {
                return ts.Seconds <= 1 ? "اخیراً" : ts.Seconds + " ثانیه پیش";
            }

            if (delta < 2 * MINUTE)
            {
                return "1 دقیقه پیش";
            }

            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " دقیقه پیش";
            }

            if (delta < 90 * MINUTE)
            {
                return "1 ساعت پیش";
            }

            if (delta < 24 * HOUR)
            {
                return ts.Hours + " ساعت پیش";
            }

            if (delta < 48 * HOUR)
            {
                return "دیروز";
            }

            if (delta < 30 * DAY)
            {
                return ts.Days + " روز پیش";
            }

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "1 ماه پیش" : months + " ماه پیش";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "1 سال پیش" : years + " سال";
            }
        }

        public static DateTime GetDateTimeHourRoundUp(this DateTime dateTime)
        {
            DateTime rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            if (dateTime.Minute > 0 || dateTime.Second > 0)
                rounded = rounded.AddHours(1);
            return rounded;
        }

        public static bool IsBetween(this DateTime? dt, DateTime start, DateTime end)
        {
            return dt.HasValue && dt.IsBetween(start, end);
        }
        public static bool IsBetween(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }
        public static bool IsBetween(this TimeSpan? dt, TimeSpan start, TimeSpan end)
        {
            return dt.HasValue && dt.Value.IsBetween(start, end);
        }
        public static bool IsBetween(this TimeSpan dt, TimeSpan start, TimeSpan end)
        {
            return dt >= start && dt <= end;
        }
        public static bool IsBetween(this DateTime? dt, TimeSpan start, TimeSpan end)
        {
            return dt.HasValue && dt.Value.IsBetween(start, end);
        }
        public static bool IsBetween(this DateTime dt, TimeSpan start, TimeSpan end)
        {
            return dt.TimeOfDay >= start && dt.TimeOfDay <= end;
        }
    }
}
