namespace CN88.Features.BetLists.Criteria.BetListOfReport
{
    using System;
    using CN88.Features.BetLists.Params;
    using Fanex.Data.Repository;

    public abstract class BetListOfReportBaseCriteria : CriteriaBase
    {
        protected BetListOfReportBaseCriteria(BetListOfReportParam param)
        {
            CustId = param.CustId;
            HeadlineId = param.HeadlineId;
            FromDate = param.FromDate;
            ToDate = param.ToDate;
            SportTypeGroups  = param.SportTypeGroupIds;
            BetTypeGroup = param.BetTypeGroup;
            LiveIndicator = param.LiveIndicator;
        }

        public int CustId { get; }

        public int HeadlineId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string SportTypeGroups { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}