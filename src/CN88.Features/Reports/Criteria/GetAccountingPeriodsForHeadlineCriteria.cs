namespace CN88.Features.Reports.Criteria
{
    using Fanex.Data.Repository;

    public class GetAccountingPeriodsForHeadlineCriteria : CriteriaBase
    {
        public GetAccountingPeriodsForHeadlineCriteria(bool isViewAllPeriod)
        {
            IsViewAllPeriod = isViewAllPeriod;
        }

        public override string GetSettingKey() => "CS_HL_Period_Sel";

        public bool IsViewAllPeriod { get; }

        public override bool IsValid() => true;
    }
}