namespace CN88.Features.BetLists.Params
{
    using System;

    public class OutrightWagerByAccountParam
    {
        public OutrightWagerByAccountParam(
            int custId,
            DateTime limitedDate,
            int roleId,
            int matchId,
            int stakeCondition,
            decimal stake)
        {
            CustId = custId;
            LimitedDate = limitedDate;
            RoleId = roleId;
            MatchId = matchId;
            StakeCondition = stakeCondition;
            Stake = stake;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }

        public int RoleId { get; }

        public int MatchId { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }
    }
}
