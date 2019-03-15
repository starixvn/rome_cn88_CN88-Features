namespace CN88.Features.BetLists.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params;

    public class WagerDetailsCriteria : CriteriaBase
    {
        public WagerDetailsCriteria(WagerDetailsParam param)
        {
            CustId = param.CustId;
            RoleId = param.RoleId;
            MatchId = param.MatchId;
            StakeCondition = param.StakeCondition;
            Stake = param.Stake;

            // must remove
            CustId = 1382348;
            RoleId = 1;
            MatchId = 7398486;
            StakeCondition = 1;
            Stake = 100;
        }

        public int CustId { get; }

        public int RoleId { get; }

        [SpParam(SqlParamName = "OrId")]
        public int MatchId { get; }

        public int StakeCondition { get; }

        public decimal Stake { get; }

        public override string GetSettingKey() => "CS_BetList_TotalBets_Outright";

        public override bool IsValid() => CustId > 0;
    }
}
