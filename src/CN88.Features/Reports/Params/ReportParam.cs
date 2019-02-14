using System;

namespace CN88.Features.Reports.Params
{
    public class ReportParam
    {
        public ReportParam(
            int reportType,
            DateTime fromDate,
            DateTime toDate,
            int sportTypeId,
            int betTypeId)
        {
            ReportType = reportType;
            FromDate = fromDate;
            ToDate = toDate;
            SportTypeId = sportTypeId;
            BetTypeId = betTypeId;
        }

        public int ReportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeId { get; }

        public int BetTypeId { get; }
    }
}