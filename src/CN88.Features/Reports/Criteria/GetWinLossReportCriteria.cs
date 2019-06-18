namespace CN88.Features.Reports.Criteria
{
    using System;
    using Shared.Models;

    public class GetWinLossReportCriteria : ReportCriteriaBase
    {
        public GetWinLossReportCriteria(
            int custId,
            bool isOwner,
            DateTime fromDate,
            DateTime toDate,
            DateTime archiveDate,
            DateTime moveBetsDay,
            int sportGroupId,
            int betTypeId,
            int isLive,
            UserLevel reportLevel)
            : base(custId, isOwner, fromDate, toDate, sportGroupId, betTypeId, isLive, reportLevel)
        {
            ArchiveDate = archiveDate;
            MoveBetsDay = moveBetsDay;
        }

        public DateTime ArchiveDate { get; }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => $"CS_Rpt_Winloss_{ReportLevel.DisplayName}";
    }
}