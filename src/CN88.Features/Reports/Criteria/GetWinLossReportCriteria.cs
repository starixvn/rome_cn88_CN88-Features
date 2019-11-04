namespace CN88.Features.Reports.Criteria
{
    using System;
    using Shared.Models;

    public class GetWinLossReportCriteria : ReportCriteriaBase
    {
        public GetWinLossReportCriteria(
            int custId,
            int subAccId,
            bool isOwner,
            bool isAllDownline,
            DateTime fromDate,
            DateTime toDate,
            DateTime moveBetsDay,
            string sportGroupIds,
            int betTypeId,
            int isLive,
            UserLevel reportLevel)
            : base(custId, subAccId, isOwner, isAllDownline, fromDate, toDate, sportGroupIds, betTypeId, isLive, reportLevel)
        {
            MoveBetsDay = moveBetsDay;
        }

        public DateTime MoveBetsDay { get; }

        public override string GetSettingKey() => $"CS_Rpt_Winloss_{ReportLevel.DisplayName}";
    }
}