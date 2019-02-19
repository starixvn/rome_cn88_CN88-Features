namespace CN88.Features.Reports.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Shared.Models;

    public class GetReportDataCriteria : CriteriaBase
    {
        public GetReportDataCriteria(
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
        {
            CustId = custId;
            IsOwner = isOwner;
            FromDate = fromDate;
            ToDate = toDate;
            BalanceUpDay = balanceUpDay;
            MoveBetsDay = moveBetsDay;
            SportGroupId = sportGroupId;
            BetTypeId = betTypeId;
            IsLive = isLive;
            ReportType = reportType;
            ReportLevel = reportLevel;
        }

        public int CustId { get; }

        public bool IsOwner { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public DateTime BalanceUpDay { get; }

        public DateTime MoveBetsDay { get; }

        [SpParam(SqlParamName = "SporttypeGroup")]
        public int SportGroupId { get; }

        [SpParam(SqlParamName = "Bettype")]
        public int BetTypeId { get; }

        public int IsLive { get; }

        public int ReportType { get; }

        [SpParam(Ignored = true)]
        public UserLevel ReportLevel { get; }

        public override string GetSettingKey()
            => ReportLevel.IsAgentLevel()
                ? "CS_Rpt_Winloss_Agent"
                : ReportLevel.IsMasterLevel()
                ? "CS_Rpt_Winloss_Master"
                : "CS_Rpt_Winloss_Super";

        public override bool IsValid() => CustId > 0 && FromDate <= ToDate;
    }
}