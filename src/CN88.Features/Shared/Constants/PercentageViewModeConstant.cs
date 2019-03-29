namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;
    using Models;

    public static class PercentageViewModeConstant
    {
        public static int FullPercentage = 0;
        public static int CompanyPercentage = 1;
        public static int SuperPercentage = 2;
        public static int MasterPercentage = 3;
        public static int AgentPercentage = 4;
        public static int SuperMasterAgentPercentage = 5;
        public static int SuperMasterPercentage = 6;
        public static int MasterAgentPercentage = 7;

        private static IEnumerable<int> superModes = new List<int>
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

        private static IEnumerable<int> masterModes = new List<int>
        {
            FullPercentage,
            MasterPercentage,
            AgentPercentage,
            MasterAgentPercentage
        };

        private static IEnumerable<int> agentModes = new List<int>
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