namespace CN88.Features.Site.BetLists.Requests.WagerMonitoring
{
    using Features.Models;
    using Features.Shared.Constants;

    public class WagerMonitoringBaseQuery : BetListBaseQuery

    {
        public WagerMonitoringBaseQuery(IUser viewer) : base(viewer)
        {
            IsSelectAllSports = true;
            StakeMode = StakeModeConstant.GreaterThan;
        }

        public int SportGroup { get; set; }

        public bool IsSelectAllSports { get; set; }

        public int StakeMode { get; set; }

        public decimal StakeLimit { get; set; }

        public string Market { get; set; }

        public string LeagueIds { get; set; }

        public string MatchIds { get; set; }

        public string BetTypeGroupIds { get; set; }

        public string SportStakeConditions { get; set; }

        public long MinTransId { get; set; }
    }
}