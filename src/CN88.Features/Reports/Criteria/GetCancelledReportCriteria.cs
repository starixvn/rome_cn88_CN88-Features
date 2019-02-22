namespace CN88.Features.Reports.Criteria
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Shared.Models;

    public class GetCancelledReportCriteria : ReportCriteriaBase
    {
        private readonly IDictionary<int, int> CancelledReportType = new Dictionary<int, int>
        {
            [ReportTypeConstants.CancelledReport] = 1,
            [ReportTypeConstants.AbnormalBets] = 2
        };

        public GetCancelledReportCriteria(
            int custId,
            bool isOwner,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            int sportGroupId,
            int betTypeId,
            int isLive,
            int reportType,
            UserLevel reportLevel)
            : base(custId, isOwner, fromDate, toDate, balanceUpDay, moveBetsDay, sportGroupId, betTypeId, isLive, reportLevel)
        {
            ReportType = CancelledReportType[reportType];
        }

        public int ReportType { get; }

        public override string GetSettingKey()
            => ReportLevel.IsMemberLevel()
                ? "CS_Rpt_Cancelled_Member"
                : ReportLevel.IsAgentLevel()
                ? "CS_Rpt_Cancelled_Agent"
                : ReportLevel.IsMasterLevel()
                ? "CS_Rpt_Cancelled_Master"
                : "CS_Rpt_Cancelled_Super";
    }
}