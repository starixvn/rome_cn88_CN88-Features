namespace CN88.Features.BetLists.Criteria
{
    using Fanex.Data.Repository;
    using Params;

    public class WagerDetailsCriteria : CriteriaBase
    {
        public WagerDetailsCriteria(WagerDetailsParam param)
        {
            CustId = param.CustId;
            RoleId = param.RoleId;
            MatchId = param.MatchId;
            BetType = param.BetTypeGroup;
            LiveIndicator = param.LiveIndicator;
            BetTeam = param.BetTeam;
            StakeCondition = param.StakeCondition;
            Stake = param.Stake;

            // must remove
            CustId = 14242821;
            RoleId = 0;
            MatchId = 28904574;
            BetType = 1;
            LiveIndicator = 0;
            BetTeam = "";
            StakeCondition = 2;
            Stake = 20;
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
