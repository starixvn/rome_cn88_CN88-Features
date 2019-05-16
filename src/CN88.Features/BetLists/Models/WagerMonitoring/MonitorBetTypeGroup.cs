namespace CN88.Features.BetLists.Models.WagerMonitoring
{
    public class MonitorBetTypeGroup
    {
        public int SportTypeGroup { get; }

        public int DisplayBetTypeGroup { get; }

        public string DisplayName { get; internal set; }

        public string BetTypeGroup { get; }

        public bool IsMainMarket { get; }
    }
}