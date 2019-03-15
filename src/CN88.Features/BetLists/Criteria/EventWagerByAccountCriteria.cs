namespace CN88.Features.BetLists.Criteria
{
    using Fanex.Data.Repository;
    using Params;

    public class EventWagerByAccountCriteria : CriteriaBase
    {
        public EventWagerByAccountCriteria(EventWagerByAccountParam param)
        {
            CustId = param.CustId;
            RoleId = param.RoleId;
            MatchId = param.MatchId;
            BetType = param.BetType;
            LiveIndicator = param.LiveIndicator;
            BetTeam = param.BetTeam;
            StakeCondition = param.StakeCondition;
            Stake = param.Stake;
        }

        public int CustId { get; }

        public int RoleId { get; }

        public int MatchId { get; }

        public int BetType { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }

        public override string GetSettingKey() => "CS_BetList_TotalBets_Match";

        public override bool IsValid() => CustId > 0;
    }
}
