namespace CN88.Features.BetLists.Helpers
{
    using System.Linq;
    using CN88.Features.Extensions;
    using static CN88.Features.BetLists.Constants.BetType;

    public static class BetTypeHelper
    {

        private static readonly int[] JackpotContributionBetType =
        {
            JackpotContribution,
            JackpotContribution_Mobile,
            JackpotContribution_Tablet
        };

        private static readonly int[] HorseRacingBetType =
        {
            VirtualHorseRacing_Win,
            VirtualHorseRacing_Place,
            VirtualHorseRacing_WinPlace
        };

        public static bool IsOutRight(int betType) => betType == Outright;

        public static bool IsHorseRacing(int betType) => HorseRacingBetType.Contains(betType);

        public static bool IsJackpotContribution(int betType) => JackpotContributionBetType.Contains(betType);

        public static bool IsLottery(int betType)
           => betType.InRange(Lottery_Max5D_60, Lottery_End);

        public static bool IsKeno(int betType)
            => betType.InRange(Keno_Start, Keno_End);

        public static bool IsHappy5(int betType)
           => betType.InRange(Happy5_Start, Happy5_End);

        public static bool IsNumberGame(int betType)
           => betType.InRange(NumberGame_Start, NumberGame_End);

        public static bool IsVirtualSport(int betType)
          => betType.InRange(VirtualSports_Start, VirtualSports_End);

        public static bool IsVirtualSport2(int betType)
          => betType.InRange(VirtualSports2_Start, VirtualSports2_End);

        public static bool NotShowComm(int betType)
            => IsKeno(betType)
            || IsNumberGame(betType)
            || IsHappy5(betType)
            || IsVirtualSport(betType)
            || IsVirtualSport2(betType)
            || IsLottery(betType);
    }
}
