namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetOtherMatchCriteria : CriteriaBase
    {
        public GetOtherMatchCriteria(bool isShowAll)
        {
            IsShowAll = isShowAll;
        }

        public bool IsShowAll { get; }

        public override string GetSettingKey() => "CS_Monitor_MatchSel";

        public override bool IsValid() => true;
    }
}