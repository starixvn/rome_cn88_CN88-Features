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
            int sportTypeId,
            int betTypeId,
            int reportType,
            int isLive,
            UserLevel reportLevel)
        {
            CustId = custId;
            IsOwner = isOwner;
            FromDate = fromDate;
            ToDate = toDate;
            SportTypeId = sportTypeId;
            BetTypeId = betTypeId;
            ReportType = reportType;
            IsLive = isLive;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public bool IsOwner { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeId { get; }

        public int BetTypeId { get; }

        public int IsLive { get; }

        public int ReportType { get; }

        public UserLevel ReportLevel { get; }
    }
}