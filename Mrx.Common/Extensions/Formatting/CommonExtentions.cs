namespace Mrx.Common.Extensions
{
    public static class CommonExtentions
    {
        public static decimal Percent(this decimal n, decimal p) => n / 100 * p;
        public static double Percent(this double n, double p) => n / 100 * p;
        public static long Percent(this long n, long p) => n / 100 * p;

        //public static IEnumerable<Type> GetSubClass(this Type type)
        //{
        //    foreach (Type t in
        //        type.Assembly.GetTypes()
        //        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(type)))
        //    {
        //        yield return t;
        //    }
        //}
        public static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        public static string SizeSuffix(this long value, int decimalPlaces = 2)
        {
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }
            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
        public static readonly string[] SizeSuffixesPersian = { "بایت", "کیلوبایت", "مگابایت", "گیگابایت", "ترابایت", "پتابایت", "اگزابایت", "زتابایت", "یتابایت" };
        public static string SizeSuffixPersian(this long value, int decimalPlaces = 2)
        {
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }
            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }
            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixesPersian[i]);
        }
    }
}
