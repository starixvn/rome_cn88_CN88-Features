namespace CN88.Features.BetLists.Criteria
{
    using Fanex.Data.Repository;
    using Params;

    public class StartedWagerDetailsCriteria : CriteriaBase
    {
        public StartedWagerDetailsCriteria(StartedWagerDetailsParam param)
        {
            CustId = param.CustId;
            RoleId = param.RoleId;
            MatchId = param.MatchId;
            BetTypeGroup = param.BetType;
            LiveIndicator = param.LiveIndicator;
            BetTeam = param.BetTeam;
            StakeCondition = param.StakeCondition;
            Stake = param.Stake;
        }

        public int CustId { get; }

        public int RoleId { get; }

        public int MatchId { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public string BetTeam { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }

        public override string GetSettingKey() => "CS_BetList_TotalBets_Started";

        public override bool IsValid() => CustId > 0;
    }
}
