namespace CN88.Features.Reports.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Shared.Models;

    public abstract class ReportCriteriaBase : CriteriaBase
    {
        protected ReportCriteriaBase(
            int custId,
            int subAccId,
            bool isOwner,
            bool isViewAll,
            DateTime fromDate,
            DateTime toDate,
            int sportGroupId,
            int betTypeGroupId,
            int isLive,
            UserLevel reportLevel)
        {
            CustId = custId;
            SubAccId = subAccId;
            IsOwner = isOwner;
            IsViewAll = isViewAll;
            FromDate = fromDate;
            ToDate = toDate;
            SportGroupId = sportGroupId;
            BetTypeGroupId = betTypeGroupId;
            IsLive = isLive;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public bool IsOwner { get; }

        public bool IsViewAll { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        [SpParam(SqlParamName = "SporttypeGroup")]
        public int SportGroupId { get; }

        [SpParam(SqlParamName = "BettypeGroup")]
        public int BetTypeGroupId { get; }

        public int IsLive { get; }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}