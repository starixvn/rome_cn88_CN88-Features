namespace CN88.Features.BetLists.Criteria.WagerMonitoring
{
    using Fanex.Data.Repository;

    public class WagerMonitoringCriteria : CriteriaBase
    {
        public WagerMonitoringCriteria(
            int custId,
            byte roleId,
            string stakeList,
            string market,
            string leagueList,
            string matchList,
            string betTypeGroupList,
            int pageSize,
            long minTransId)
        {
            CustId = custId;
            RoleId = roleId;
            StakeList = stakeList;
            Market = market;
            LeagueList = leagueList;
            MatchList = matchList;
            BetTypeGroupList = betTypeGroupList;
            PageSize = pageSize;
            MinTransId = minTransId;
        }

        public int CustId { get; }

        public byte RoleId { get; }

        public string StakeList { get; }

        public string Market { get; }

        public string LeagueList { get; }

        public string MatchList { get; }

        public string BetTypeGroupList { get; }

        public int PageSize { get; }

        public long MinTransId { get; set; }

        public override string GetSettingKey() => "CS_Monitor_BetList";

        public override bool IsValid() => CustId > 0 && RoleId > 0;
    }
}