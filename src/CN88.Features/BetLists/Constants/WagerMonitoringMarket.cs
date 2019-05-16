namespace CN88.Features.BetLists.Constants
{
    using System.Collections.Generic;

    public static class WagerMonitoringMarket
    {
        public const int InPlay = 1;
        public const int Today = 2;
        public const int Early = 3;

        public static IDictionary<int, string> Markets { get; } = new Dictionary<int, string>
        {
            [InPlay] = nameof(InPlay),
            [Today] = nameof(Today),
            [Early] = nameof(Early)
        };
    }
}