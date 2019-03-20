namespace CN88.Features.BetLists.Constants
{
    using System.Collections.Generic;

    public static class TotalBetParlaySport
    {
        public const int All = 0;
        public const int Soccer = 1;
        public const int NonSoccer = 2;
        public const int MixSports = 3;

        public static IEnumerable<int> Sports =
            new List<int> { All, Soccer, NonSoccer, MixSports };
    }
}
