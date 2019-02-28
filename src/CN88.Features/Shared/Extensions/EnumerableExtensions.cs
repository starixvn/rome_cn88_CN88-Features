using System.Collections.Generic;

namespace CN88.Features.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static string JoinWithComma<T>(this IEnumerable<T> source) => string.Join(",", source);
    }
}