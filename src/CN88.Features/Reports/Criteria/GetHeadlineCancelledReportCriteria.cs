namespace CN88.Features.Reports.Criteria
{
    using System;
    using System.Collections.Generic;
    using CN88.Features.Reports.Constants;
    using CN88.Features.Shared.Models;

    public class GetHeadlineCancelledReportCriteria : GetHeadlineWinLossReportCriteria
    {
        private readonly IDictionary<int, int> CancelledReportType = new Dictionary<int, int>
        {
            [ReportTypeConstants.CancelledReport] = 1,
            [ReportTypeConstants.AbnormalBets] = 2
        };

        public GetHeadlineCancelledReportCriteria(
            int headlineId,
            int custId,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            string sportGroupIds,
            int betTypeGroupId,
            int isLive,
            int reportType,
            UserLevel reportLevel)
            : base(headlineId, custId, fromDate, toDate, moveBetsDay, sportGroupIds, betTypeGroupId, isLive, reportLevel)
        {
            ReportType = CancelledReportType[reportType];
        }

        public int ReportType { get; }

        public override string GetSettingKey() => $"CS_HL_Rpt_Cancelled_{ReportLevel.DisplayName}";
    }
}
