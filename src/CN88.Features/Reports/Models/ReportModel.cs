namespace CN88.Features.Reports.Models
{
    public class ReportModel
    {
        public ReportModel()
        {
        }

        public ReportModel(
            int custId,
            string username,
            string name,
            int wagers,
            decimal stake,
            decimal validStake,
            decimal memberIncome,
            decimal agent,
            decimal agentResult,
            decimal master,
            decimal masterResult,
            decimal super,
            decimal superIncome,
            decimal superResult,
            decimal superStock,
            decimal headlinePoss,
            decimal headlineIncome,
            decimal headlineResult,
            decimal headlineStock)
        {
            CustId = custId;
            Username = username;
            Name = name;
            Wagers = wagers;
            Stake = stake;
            ValidStake = validStake;
            MemberIncome = memberIncome;

            Agent = agent;
            AgentResult = agentResult;
            Master = master;
            MasterResult = masterResult;

            Super = super;
            SuperIncome = superIncome;
            SuperResult = superResult;
            SuperStock = superStock;

            HeadlinePoss = headlinePoss;
            HeadlineIncome = headlineIncome;
            HeadlineResult = headlineResult;
            HeadlineStock = headlineStock;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Name { get; }

        public int Wagers { get; }

        public decimal Stake { get; }

        public decimal ValidStake { get; }

        public decimal MemberIncome { get; }

        public decimal Agent { get; }

        public decimal AgentResult { get; }

        public decimal Master { get; }

        public decimal MasterResult { get; }

        public decimal Super { get; }

        public decimal SuperIncome { get; }

        public decimal SuperResult { get; }

        public decimal SuperStock { get; }

        public decimal HeadlinePoss { get; }

        public decimal HeadlineIncome { get; }

        public decimal HeadlineResult { get; }

        public decimal HeadlineStock { get; }
    }
}