using System;
using CN88.Features.Shared.Models;

namespace CN88.Features.Reports.Criteria
{
    public class GetWinLossReportCriteria : ReportCriteriaBase
    {
        public GetWinLossReportCriteria(
            int custId,
            bool isOwner,
            DateTime fromDate,
            DateTime toDate,
            DateTime balanceUpDay,
            DateTime moveBetsDay,
            int sportGroupId,
            int betTypeId,
            int isLive,
            UserLevel reportLevel)
            : base(custId, isOwner, fromDate, toDate, balanceUpDay, moveBetsDay, sportGroupId, betTypeId, isLive, reportLevel)
        {
        }

        public override string GetSettingKey()
            => ReportLevel.IsMemberLevel()
                ? "CS_Rpt_Winloss_Member"
                : ReportLevel.IsAgentLevel()
                ? "CS_Rpt_Winloss_Agent"
                : ReportLevel.IsMasterLevel()
                ? "CS_Rpt_Winloss_Master"
                : "CS_Rpt_Winloss_Super";
    }
}