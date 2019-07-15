namespace CN88.Features.Reports.Params
{
    using System;
    using Shared.Models;

    public class ReportParam
    {
        public ReportParam(
            int custId,
            int subAccId,
            bool isOwner,
            bool isViewAll,
            DateTime fromDate,
            DateTime toDate,
            int sportGroupId,
            int betTypeId,
            int reportType,
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
            BetTypeId = betTypeId;
            ReportType = reportType;
            IsLive = isLive;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public bool IsOwner { get; }

        public bool IsViewAll { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportGroupId { get; }

        public int BetTypeId { get; }

        public int IsLive { get; }

        public int ReportType { get; }

        public UserLevel ReportLevel { get; }
    }
}