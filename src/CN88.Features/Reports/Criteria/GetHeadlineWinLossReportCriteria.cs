namespace CN88.Features.Reports.Criteria
{
    using System;
    using CN88.Features.Shared.Models;

    public class GetHeadlineWinLossReportCriteria : HeadlineReportCriteriaBase
    {
        public GetHeadlineWinLossReportCriteria(
            int headlineId,
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            string sportGroupIds,
            int betTypeGroupId,
            int isLive,
            UserLevel reportLevel)
            : base(headlineId, custId, fromDate, toDate, sportGroupIds, betTypeGroupId, isLive, reportLevel)
        {
            MoveBetsDay = moveBetsDay;
        }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => $"CS_HL_Rpt_Winloss_{ReportLevel.DisplayName}";
    }
}
