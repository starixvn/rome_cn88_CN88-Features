namespace CN88.Features.Site.Reports.Constants
{
    using System.Collections.Generic;
    using Features.Shared.Models;

    public static class ReportSortPropertyConstants
    {
        private static IEnumerable<string> superSortProperties = new List<string>
        {
            "Username",
            "SMA",
            "SMAIncome",
            "SMAResult",
            "SMAStock",
            "SMAStockPercent",
            "HeadlinePoss",
            "HeadlineIncome",
            "HeadlineResult",
            "HeadlineStock",
            "HeadlineStockPercent",
            "Wagers",
            "Stake",
            "ValidStake",
            "MemberIncome",
            "Agent",
            "AgentResult",
            "MasterAgent"
        };

        private static IEnumerable<string> masterSortProperties = new List<string>
        {
            "Username",
            "Name",
            "MAIncome",
            "MAResult",
            "MAStock",
            "MAStockPercent",
            "SMA",
            "SMAPoss",
            "SMAIncome",
            "SMAResult",
            "SMAStock",
            "SMAStockPercent",
            "Wagers",
            "Stake",
            "ValidStake",
            "MemberIncome",
            "Agent",
            "AgentResult"
        };

        private static IEnumerable<string> agentSortProperties = new List<string>
        {
            "Username",
            "Name",
            "Agent",
            "AgentPoss",
            "AgentIncome",
            "AgentResult",
            "AgentStock",
            "MAPoss",
            "MAIncome",
            "MAResult",
            "Wagers",
            "Stake",
            "ValidStake",
            "MemberIncome"
        };

        private static IEnumerable<string> memberSortProperties = new List<string>
        {
            "Username",
            "Name",
            "Wagers",
            "Stake",
            "ValidStake",
            "MemberIncome",
            "AgentPoss",
            "AgentIncome",
            "AgentResultA"
        };

        public static IDictionary<UserLevel, IEnumerable<string>> SortPropertiesByLevel { get; } = new Dictionary<UserLevel, IEnumerable<string>>
        {
            [UserLevel.Super] = superSortProperties,
            [UserLevel.Master] = masterSortProperties,
            [UserLevel.Agent] = agentSortProperties,
            [UserLevel.Member] = memberSortProperties,
        };
    }
}