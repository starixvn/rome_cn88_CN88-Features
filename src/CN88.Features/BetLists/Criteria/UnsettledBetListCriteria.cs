namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data.Repository;
    using Params;

    public class UnsettledBetListCriteria : CriteriaBase
    {
        public UnsettledBetListCriteria(BetListOfReportParam param)
        {
            CustId = param.CustId;
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroup = param.SportTypeGroup;
            BetTypeGroup = param.BetTypeGroup;
            LiveIndicator = param.LiveIndicator;
        }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public override string GetSettingKey() => "CS_BetList_Unsettled_Mem";

        public override bool IsValid() => CustId > 0;
    }
}