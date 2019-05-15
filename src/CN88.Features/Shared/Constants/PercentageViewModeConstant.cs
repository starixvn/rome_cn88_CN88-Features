namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;
    using Models;

    public static class PercentageViewModeConstant
    {
        public static readonly int FullPercentage;
        public static readonly int CompanyPercentage = 1;
        public static readonly int SuperPercentage = 2;
        public static readonly int MasterPercentage = 3;
        public static readonly int AgentPercentage = 4;
        public static readonly int SuperMasterAgentPercentage = 5;
        public static readonly int SuperMasterPercentage = 6;
        public static readonly int MasterAgentPercentage = 7;

        private static readonly IEnumerable<int> superModes = new List<int>
        {
            FullPercentage,
            CompanyPercentage,
            SuperPercentage,
            MasterPercentage,
            AgentPercentage,
            SuperMasterAgentPercentage,
            SuperMasterPercentage,
            MasterAgentPercentage
        };

        private static readonly IEnumerable<int> masterModes = new List<int>
        {
            FullPercentage,
            MasterPercentage,
            AgentPercentage,
            MasterAgentPercentage
        };

        private static readonly IEnumerable<int> agentModes = new List<int>
        {
            FullPercentage,
            AgentPercentage
        };

        public static IDictionary<UserLevel, IEnumerable<int>> ModesByLevel { get; } = new Dictionary<UserLevel, IEnumerable<int>>
        {
            [UserLevel.Super] = superModes,
            [UserLevel.Master] = masterModes,
            [UserLevel.Agent] = agentModes
        };
    }
}