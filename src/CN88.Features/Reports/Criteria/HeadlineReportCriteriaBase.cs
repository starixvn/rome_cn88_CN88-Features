namespace CN88.Features.Reports.Criteria
{
    using System;
    using CN88.Features.Shared.Models;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public abstract class HeadlineReportCriteriaBase : CriteriaBase
    {
        protected HeadlineReportCriteriaBase(
            int headlineId,
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string sportGroupIds,
            int betTypeGroupId,
            int isLive,
            UserLevel reportLevel)
        {
            HeadlineId = headlineId;
            CustId = custId;
            FromDate = fromDate;
            ToDate = toDate;
            SportGroupIds = sportGroupIds;
            BetTypeGroupId = betTypeGroupId;
            IsLive = isLive;
            ReportLevel = reportLevel;
        }

        public int HeadlineId { get; }

        public int CustId { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "SporttypeGroups")]
        public string SportGroupIds { get; }

        [SpParam(SqlParamName = "BettypeGroup")]
        public int BetTypeGroupId { get; }

        public int IsLive { get; }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override bool IsValid() => HeadlineId > 0 && CustId > 0 && FromDate <= ToDate;
    }
}
