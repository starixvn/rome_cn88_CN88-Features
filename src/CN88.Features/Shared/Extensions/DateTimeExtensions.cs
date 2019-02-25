namespace CN88.Features.Shared.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";
        private const string ShortDate = "yyyy-MM-dd";

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);

        public static string ToShortDate(this DateTime dateTime)
            => dateTime.ToString(ShortDate);
    }
}