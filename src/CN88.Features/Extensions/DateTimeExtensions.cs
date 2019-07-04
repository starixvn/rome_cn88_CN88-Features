namespace CN88.Features.Extensions
{
    using System;
    using System.Resources;

    public static class DateTimeExtensions
    {
        public const string ShortDateFormat = "yyyy-MM-dd";

        public const string TimeFormat = "HH:mm:ss";
        private const string LongTime24h = "HH:mm:ss";
        private const string LongDateTime24h = "yyyy-MM-dd HH:mm:ss";
        private const string GlobalShowtime = "yyyy-MM-dd HH:mm";

        public static string ToShortDateFormat(this DateTime date)
            => date.ToString(ShortDateFormat);

        public static string ToLongDateFormat(this DateTime dateTime)
           => $"{ToShortDateFormat(dateTime)} {ToTimeFormat(dateTime)}";

        public static string ToTimeFormat(this DateTime date)
            => date.ToString(TimeFormat);

        public static string ToLongDay(this DateTime date, ResourceManager resourceManager)
            => $"{date.ToShortDateFormat()} {resourceManager.GetString(date.ToString("dddd"))}";

        public static string ToLongTime24h(this DateTime dateTime)
            => dateTime.ToString(LongTime24h);

        public static string ToLongDateTime24h(this DateTime dateTime)
            => dateTime.ToString(LongDateTime24h);

        public static string ToGlobalShowtime(this DateTime? dateTime)
            => dateTime?.ToString(GlobalShowtime);

        public static DateTime GetGreaterDate(this DateTime fromDate, DateTime limitedDate)
            => fromDate < limitedDate ? limitedDate : fromDate;

        public static bool IsBefore(this DateTime dateTime, DateTime otherDateTime)
            => dateTime < otherDateTime;

        public static bool IsBeforeOrEqual(this DateTime dateTime, DateTime otherDateTime)
            => dateTime <= otherDateTime;
    }
}
