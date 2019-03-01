namespace CN88.Features.Shared.Extensions
{
    using System.Collections.Generic;

    public static class EnumerableExtensions
    {
        public static string JoinWithComma<T>(this IEnumerable<T> source) => string.Join(",", source);
    }
}