namespace CN88.Features.BetLists.Params
{
    using System;

    public class WagerDetailsParam
    {
        public WagerDetailsParam(
            int custId,
            DateTime limitedDate,
            int roleId,
            int matchId,
            int betTypeGroup,
            int liveIndicator,
            string betTeam,
            int stakeCondition,
            decimal stake)
        {
            CustId = custId;
            LimitedDate = limitedDate;
            RoleId = roleId;
            MatchId = matchId;
            BetTypeGroup = betTypeGroup;
            LiveIndicator = liveIndicator;
            BetTeam = betTeam;
            StakeCondition = stakeCondition;
            Stake = stake;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }

        public int RoleId { get; }

        public int MatchId { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }
    }
}
