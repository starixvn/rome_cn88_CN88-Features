namespace CN88.Features.BetLists.Models.WagerMonitoring
{
    public class MatchInfo
    {
        public int MatchId { get; }

        public string MatchCode { get; }

        public string MatchName { get; internal set; }

        public int SportType { get; }

        public int LeagueId { get; }

        public int HomeId { get; }

        public int AwayId { get; }

        public string Race { get; }

        public string ShowTime { get; }

        public string Status { get; }

        public bool IsNeutral { get; }

        public string BetStatus { get; }

        public byte SettlementStatus { get; }

        public int SportTypeGroup { get; }
    }
}