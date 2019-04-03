namespace CN88.Features.BetLists.Helpers
{
    using System.Linq;
    using Constants;

    public static class BetTypeHelper
    {
        private static readonly int[] JackpotContributionBetType =
        {
            BetTypeIdConstants.JackpotContribution,
            BetTypeIdConstants.JackpotContribution_Mobile,
            BetTypeIdConstants.JackpotContribution_Tablet
        };

        private static readonly int[] HorseRacingBetType =
        {
            BetTypeIdConstants.VirtualHorseRacing_Win,
            BetTypeIdConstants.VirtualHorseRacing_Place,
            BetTypeIdConstants.VirtualHorseRacing_WinPlace
        };

        public static bool IsOutRight(int betType) => betType == BetTypeIdConstants.Outright;

        public static bool IsHorseRacing(int betType) => HorseRacingBetType.Contains(betType);

        public static bool IsJackpotContribution(int betType) => JackpotContributionBetType.Contains(betType);

        public static bool NotShowComm(int betType)
            => (BetTypeIdConstants.Keno_Start <= betType && betType <= BetTypeIdConstants.Keno_End) ||
            (BetTypeIdConstants.Lottery_Max5D_60 <= betType && betType <= BetTypeIdConstants.Lottery_End) ||
            (BetTypeIdConstants.NumberGame_Start <= betType && betType <= BetTypeIdConstants.NumberGame_End) ||
            (BetTypeIdConstants.Happy5_Start <= betType && betType <= BetTypeIdConstants.Happy5_End) ||
            (BetTypeIdConstants.VirtualSports_Start <= betType && betType <= BetTypeIdConstants.VirtualSports_End) ||
            (BetTypeIdConstants.VirtualSports2_Start <= betType && betType <= BetTypeIdConstants.VirtualSports2_End);
    }
}
