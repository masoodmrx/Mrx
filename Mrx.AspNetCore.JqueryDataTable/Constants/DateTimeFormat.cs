namespace Mrx.AspNetCore.JqueryDataTable
{
    public static class DateTimeFormat
    {
        public const string ShortDatePattern = "jYYYY/jMM/jDD";
        public const string LongDatePattern = "dddd, jD jMMMM jYYYY";
        public const string ShortTimePattern = "HH:mm";
        public const string LongTimePattern = "HH:mm:ss";
        public const string HtmlShortDateTimePattern = ShortDatePattern + "<br>" + ShortTimePattern;
        public const string HtmlLongDateTimePattern = LongDatePattern + "<br>" + LongTimePattern;
        public const string ShortDateTimePattern = ShortDatePattern + "\r\n" + ShortTimePattern;
        public const string LongDateTimePattern = LongDatePattern + "\r\n" + LongTimePattern;
    }
}
