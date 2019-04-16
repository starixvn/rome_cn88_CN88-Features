namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetOutrightMatchCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "CS_Monitor_OutrightSel";

        public override bool IsValid() => true;
    }
}