namespace CN88.Features.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "yyyy-MM-dd";

        public const string TimeFormat = "HH:mm:ss";

        public static string ToShortDateFormat(this DateTime date)
        {
            return date.ToString(ShortDateFormat);
        }

        public static string ToTimeFormat(this DateTime date)
        {
            return date.ToString(TimeFormat);
        }
    }
}
