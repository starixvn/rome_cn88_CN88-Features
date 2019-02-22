namespace CN88.Features.Reports.Params
{
    using System;
    using Shared.Models;

    public class ReportParam
    {
        public ReportParam(
            int custId,
            bool isOwner,
            DateTime fromDate,
            DateTime toDate,
            int sportGroupId,
            int betTypeId,
            int reportType,
            int isLive,
            UserLevel reportLevel)
        {
            CustId = custId;
            IsOwner = isOwner;
            FromDate = fromDate;
            ToDate = toDate;
            SportGroupId = sportGroupId;
            BetTypeId = betTypeId;
            ReportType = reportType;
            IsLive = isLive;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public bool IsOwner { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportGroupId { get; }

        public int BetTypeId { get; }

        public int IsLive { get; }

        public int ReportType { get; }

        public UserLevel ReportLevel { get; }
    }
}