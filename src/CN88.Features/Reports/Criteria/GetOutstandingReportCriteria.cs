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
            DateTime moveBetsDay,
            int sportGroupId,
            int betTypeId,
            int isLive,
            UserLevel reportLevel)
            : base(custId, isOwner, fromDate, toDate, sportGroupId, betTypeId, isLive, reportLevel)
        {
        }

        public override string GetSettingKey()
            => ReportLevel.IsMemberLevel()
                ? "CS_Rpt_Outstanding_Member"
                : ReportLevel.IsAgentLevel()
                ? "CS_Rpt_Outstanding_Agent"
                : ReportLevel.IsMasterLevel()
                ? "CS_Rpt_Outstanding_Master"
                : "CS_Rpt_Outstanding_Super";
    }
}