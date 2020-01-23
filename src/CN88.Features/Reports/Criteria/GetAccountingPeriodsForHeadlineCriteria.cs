namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetAccountingPeriodsForHeadlineCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_HL_AccountingPeriod_Sel";

        public override bool IsValid() => true;
    }
}