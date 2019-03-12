namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data.Repository;
    using Params;

    public class BetListByAccountCriteria : CriteriaBase
    {
        public BetListByAccountCriteria(BetListByAccountParam param, DateTime moveBetsDate)
        {
            CustId = param.CustId;
            MoveBetsDay = moveBetsDate;

            // must remove
            CustId = 34239892; // 34239336;
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

        public override string GetSettingKey() => "CS_BetList_WinLoss_Mem";

        public override bool IsValid() => CustId > 0;
    }
}
