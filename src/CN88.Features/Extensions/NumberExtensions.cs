namespace CN88.Features.Extensions
{
    using System;

    public static class NumberExtensions
    {        
        public static string ToNumberWithoutDecimal(this DateTime date)
            => date.ToString("#,##0");
    }
}
