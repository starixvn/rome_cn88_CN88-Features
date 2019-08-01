namespace CN88.Features.BetLists.Params
{
    using System;

    public class GetTotalBetParlayParam
    {
        public GetTotalBetParlayParam(
            int custId,
            DateTime limitedDate,
            int roleId,
            int subAccId,
            bool isAllDownline,
            int sportGroupId,
            int viewMode,
            int stakeMode,
            decimal stakeLimit)
        {
            CustId = custId;
            LimitedDate = limitedDate;
            RoleId = roleId;
            SubAccId = subAccId;
            IsAllDownline = isAllDownline;
            SportGroupId = sportGroupId;
            ViewMode = viewMode;
            StakeMode = stakeMode;
            StakeLimit = stakeLimit;
        }

        public int CustId { get; }

        public DateTime LimitedDate { get; }

        public int RoleId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; }

        public int SportGroupId { get; }

        public int StakeMode { get; }

        public decimal StakeLimit { get; }

        public int ViewMode { get; }
    }
}