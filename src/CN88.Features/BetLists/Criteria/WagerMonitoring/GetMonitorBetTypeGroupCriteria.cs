namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetMonitorBetTypeGroupCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_Monitor_BetTypeSel";

        public override bool IsValid() => true;
    }
}