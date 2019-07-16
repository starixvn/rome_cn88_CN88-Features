namespace CN88.Features.BetLists.Params
{
    using System;

    public class EventWagerParam
    {
        public EventWagerParam(
            int custId,
            DateTime limitedDate,
            int roleId,
            int subAccId,
            bool isAllDownline,
            int matchId,
            int betType,
            int liveIndicator,
            string betTeam,
            int stakeCondition,
            decimal stake)
        {
            CustId = custId;
            LimitedDate = limitedDate;
            RoleId = roleId;
            SubAccId = subAccId;
            IsAllDownline = isAllDownline;
            MatchId = matchId;
            BetType = betType;
            LiveIndicator = liveIndicator;
            BetTeam = betTeam;
            StakeCondition = stakeCondition;
            Stake = stake;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }

        public int RoleId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; }

        public int MatchId { get; }

        public int BetType { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }
    }
}