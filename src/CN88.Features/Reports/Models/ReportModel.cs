namespace CN88.Features.Reports.Models
{
    public class ReportModel
    {
        public ReportModel()
        {
        }

        public ReportModel(
            int userId,
            string username,
            string name,
            int wagers,
            decimal stake,
            decimal validStake,
            decimal member,
            decimal memberCurrency,
            decimal agent,
            decimal agentPoss,
            decimal agentResult,
            decimal agentCurrency,
            decimal agentStock,
            decimal master,
            decimal masterPoss,
            decimal masterResult,
            decimal masterStock,
            decimal masterStockPercent,
            decimal super,
            decimal superPoss,
            decimal superResult,
            decimal superStock,
            decimal superStockPercent)
        {
        }

        public int UserId { get; }

        public string Username { get; }

        public string Name { get; }

        public int Wagers { get; }

        public decimal Stake { get; }

        public decimal ValidStake { get; }

        public decimal Member { get; }

        public decimal MemberCurrency { get; }

        public decimal Agent { get; }

        public decimal AgentPoss { get; }

        public decimal AgentResult { get; }

        public decimal AgentCurrency { get; }

        public decimal AgentStock { get; }

        public decimal Master { get; } // Master Agent

        public decimal MasterPoss { get; } // MA Poss.

        public decimal MasterResult { get; } // MA Result

        public decimal MasterStock { get; } // MA Stock

        public decimal MasterStockPercent { get; } // MA Stock %

        public decimal Super { get; } // SMA

        public decimal SuperPoss { get; } // SMA Poss.

        public decimal SuperResult { get; } // SMA Result

        public decimal SuperStock { get; } // SMA Stock

        public decimal SuperStockPercent { get; } // SMA Stock %
    }
}