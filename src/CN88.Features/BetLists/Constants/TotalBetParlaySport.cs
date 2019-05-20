namespace CN88.Features.BetLists.Constants
{
    using System.Collections.Generic;

    public static class TotalBetParlaySport
    {
        public static readonly int All;
        public static readonly int Soccer = 1;
        public static readonly int NonSoccer = 2;
        public static readonly int MixSports = 3;

        public static IEnumerable<int> Sports { get; } = new List<int> { All, Soccer, NonSoccer, MixSports };
    }
}