namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;
    using Models;

    public static class PercentageViewModeConstant
    {
        public const int FullPercentage = 0;
        public const int CompanyPercentage = 1;
        public const int SuperPercentage = 2;
        public const int MasterPercentage = 3;
        public const int AgentPercentage = 4;
        public const int SuperMasterAgentPercentage = 5;
        public const int SuperMasterPercentage = 6;
        public const int MasterAgentPercentage = 7;

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