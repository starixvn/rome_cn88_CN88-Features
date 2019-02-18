namespace CN88.Features.Shared.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDateWithSlash = "MM/dd/yyyy";

        public static string ToShortDateWithSlash(this DateTime dateTime)
            => dateTime.ToString(ShortDateWithSlash);
    }
}