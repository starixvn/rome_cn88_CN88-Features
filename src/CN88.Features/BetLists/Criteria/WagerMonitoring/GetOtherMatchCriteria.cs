namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetOtherMatchCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_Monitor_MatchSel";

        public override bool IsValid() => true;
    }
}