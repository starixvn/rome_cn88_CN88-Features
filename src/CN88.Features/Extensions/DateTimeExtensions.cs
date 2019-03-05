namespace CN88.Features.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "yyyy-MM-dd";

        public const string TimeFormat = "HH:mm:ss";
        private const string LongTime24h = "HH:mm:ss";

        public static string ToShortDateFormat(this DateTime date)
            => date.ToString(ShortDateFormat);

        public static string ToLongDateFormat(this DateTime dateTime)
           => $"{ToShortDateFormat(dateTime)} {ToTimeFormat(dateTime)}";

        public static string ToTimeFormat(this DateTime date)
            => date.ToString(TimeFormat);

        public static string ToLongDay(this DateTime date)
            => date.ToString("YYYY-MM-dd dddd");

        public static string ToLongTime24h(this DateTime dateTime)
            => dateTime.ToString(LongTime24h);
    }
}
