namespace CN88.Features.Reports.Criteria
{
    using System;
    using CN88.Features.Shared.Models;

    public class GetOutstandingReportCriteria : ReportCriteriaBase
    {
        public GetOutstandingReportCriteria(
            int custId,
            bool isOwner,
            DateTime fromDate,
            DateTime toDate,
            int sportGroupId,
            int betTypeId,
            int isLive,
            UserLevel reportLevel)
            : base(custId, isOwner, fromDate, toDate, sportGroupId, betTypeId, isLive, reportLevel)
        {
        }

        public override string GetSettingKey() => $"CS_Rpt_Outstanding_{ReportLevel.DisplayName}";
    }
}