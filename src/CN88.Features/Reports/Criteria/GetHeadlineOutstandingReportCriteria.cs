namespace CN88.Features.Reports.Criteria
{
    using System;
    using CN88.Features.Shared.Models;

    public class GetHeadlineOutstandingReportCriteria : HeadlineReportCriteriaBase
    {
        public GetHeadlineOutstandingReportCriteria(
            int headlineId,
            int custId,
            DateTime fromDate,
            DateTime toDate,
            string sportGroupIds,
            int betTypeGroupId,
            int isLive,
            UserLevel reportLevel)
            : base(headlineId, custId, fromDate, toDate, sportGroupIds, betTypeGroupId, isLive, reportLevel)
        {
        }

        public override string GetSettingKey() => $"CS_HL_Rpt_Outstanding_{ReportLevel.DisplayName}";
    }
}
