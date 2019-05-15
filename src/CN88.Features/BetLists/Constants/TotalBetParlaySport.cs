namespace CN88.Features.BetLists.Constants
{
    using System.Collections.Generic;

    public static class TotalBetParlaySport
    {
        public static int All = 0;
        public static int Soccer = 1;
        public static int NonSoccer = 2;
        public static int MixSports = 3;

        public static IEnumerable<int> Sports =
            new List<int> { All, Soccer, NonSoccer, MixSports };
    }
}
