namespace CN88.Features.BetLists.Params
{
    using System;

    public class WagerDetailsParam
    {
        public WagerDetailsParam(
            int custId,
            DateTime limitedDate,
            int roleId,
            int outrightId,
            int stakeCondition,
            decimal stake)
        {
            CustId = custId;
            LimitedDate = limitedDate;
            RoleId = roleId;
            OutrightId = outrightId;
            StakeCondition = stakeCondition;
            Stake = stake;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }

        public int RoleId { get; }

        public int OutrightId { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }
    }
}
