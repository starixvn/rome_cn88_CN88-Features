namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetAccountingPeriodsForAgencyCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_AccountingPeriod_Sel";

        public override bool IsValid() => true;
    }
}