namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params;

    public class TotalBetParlayCriteria : CriteriaBase
    {
        public TotalBetParlayCriteria(GetTotalBetParlayParam param, DateTime moveBetsDate)
        {
            CustId = param.CustId;
            SportGroupId = param.SportGroupId;
            ViewMode = param.ViewMode;
            StakeMode = param.StakeMode;
            StakeLimit = param.StakeLimit;

            MoveBetsDay = moveBetsDate;

            // must remove
            CustId = 34239336;
            FromDate = new DateTime(2019, 3, 11);
            ToDate = new DateTime(2019, 3, 11);
            SportTypeGroup = 0;
            BetTypeGroup = 0;
            LiveIndicator = 2;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime MoveBetsDay { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        [SpParam(Ignored = true)]
        public int SportGroupId { get; }

        [SpParam(Ignored = true)]
        public int ViewMode { get; }

        [SpParam(Ignored = true)]
        public int StakeMode { get; }

        [SpParam(Ignored = true)]
        public decimal StakeLimit { get; }

        public override string GetSettingKey() => "CS_BetList_WinLoss_Mem";

        public override bool IsValid() => CustId > 0;
    }
}

