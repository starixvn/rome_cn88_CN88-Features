namespace CN88.Features.BetLists.Criteria
{
    using System;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Params;

    public class TotalBetParlayCriteria : CriteriaBase
    {
        public TotalBetParlayCriteria(GetTotalBetParlayParam param, DateTime moveBetsDate)
        {
            CustId = param.CustId;
            RoleId = param.RoleId;
            SportGroupId = param.SportGroupId;
            StakeMode = param.StakeMode;
            StakeLimit = param.StakeLimit;
            ViewMode = param.ViewMode;
        }

        public int CustId { get; }

        public int RoleId { get; }

        [SpParam(SqlParamName = "Sport")]
        public int SportGroupId { get; }

        [SpParam(SqlParamName = "StakeCondition")]
        public int StakeMode { get; }

        [SpParam(SqlParamName = "Stake")]
        public decimal StakeLimit { get; }

        [SpParam(SqlParamName = "PossType")]
        public int ViewMode { get; }

        public override string GetSettingKey() => "CS_BetList_TotalBets_Parlay";

        public override bool IsValid() => CustId > 0 && RoleId >= 0;
    }
}
