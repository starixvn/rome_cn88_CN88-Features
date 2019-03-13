namespace CN88.Features.BetLists.Params
{
    using System;

    public class GetTotalBetParlayParam
    {
        public GetTotalBetParlayParam(
            int custId,
            DateTime limitedDate,
            int sportGroupId,
            int viewMode,
            int stakeMode,
            decimal stakeLimit)
        {
            CustId = custId;
            LimitedDate = limitedDate;
            SportGroupId = sportGroupId;
            ViewMode = viewMode;
            StakeMode = stakeMode;
            StakeLimit = stakeLimit;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }

        public int SportGroupId { get; }

        public int ViewMode { get; }

        public int StakeMode { get; }

        public decimal StakeLimit { get; }
    }
}
