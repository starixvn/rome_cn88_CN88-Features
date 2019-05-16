namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetMonitorSportGroupCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_Monitor_SportSel";

        public override bool IsValid() => true;
    }
}