namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;

    public static class PercentageViewModeConstant
    {
        public static int FullPercentage = 1;
        public static int CompanyPercentage = 2;
        public static int SuperPercentage = 3;
        public static int MasterPercentage = 4;
        public static int AgentPercentage = 5;
        public static int SuperMasterAgentPercentage = 6;
        public static int SuperMasterPercentage = 7;
        public static int MasterAgentPercentage = 8;

        public static IEnumerable<int> Modes = new List<int>
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
    }
}
