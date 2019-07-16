namespace CN88.Features.Shared.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringExtensions
    {
        public static string FormatNumber(this decimal numberValue)
            => numberValue.ToString("#,##0.00");

        public static IEnumerable<int> ToListInt(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Enumerable.Empty<int>();
            }

            var result = 0;

            var ids = value
                .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Where(str => int.TryParse(str, out result))
                .Select(str => Convert.ToInt32(str));

            return ids;
        }

        public static string[] SplitString(this string value)
            => value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
    }
}