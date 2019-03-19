namespace CN88.Features.Shared.Constants
{
    using System.Collections.Generic;

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
