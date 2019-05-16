namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class GetOutrightMatchCriteria : CriteriaBase
    {
        public GetOutrightMatchCriteria(bool isShowAll)
        {
            IsShowAll = isShowAll;
        }

        public bool IsShowAll { get; }

        public override string GetSettingKey() => "CS_Monitor_OutrightSel";

        public override bool IsValid() => true;
    }
}