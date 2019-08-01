namespace CN88.Features.BetLists.Params
{
    public class WagerMonitoringParam
    {
        public WagerMonitoringParam(
            int custId,
            byte custLevelId,
            int subAccId,
            bool isAllDownline,
            int sportGroupId,
            string stakeList,
            string market,
            string leagueIds,
            string matchIds,
            string displayBetTypeGroupIds,
            long minTransId)
        {
            CustId = custId;
            CustLevelId = custLevelId;
            SubAccId = subAccId;
            IsAllDownline = isAllDownline;
            SportGroupId = sportGroupId;
            StakeConditons = stakeList;
            Market = market;
            LeagueIds = leagueIds;
            MatchIds = matchIds;
            DisplayBetTypeGroupIds = displayBetTypeGroupIds;
            MinTransId = minTransId;
        }

        public int CustId { get; }

        public byte CustLevelId { get; }

        public int SubAccId { get; }

        public bool IsAllDownline { get; }

        public int SportGroupId { get; }

        public string StakeConditons { get; }

        public string Market { get; }

        public string LeagueIds { get; }

        public string MatchIds { get; }

        public string DisplayBetTypeGroupIds { get; }

        public long MinTransId { get; set; }
    }
}