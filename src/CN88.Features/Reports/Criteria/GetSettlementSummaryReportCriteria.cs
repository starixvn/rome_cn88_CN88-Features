namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetSettlementSummaryReportCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_Rpt_SettlementSummary";

        public override bool IsValid() => true;
    }
}