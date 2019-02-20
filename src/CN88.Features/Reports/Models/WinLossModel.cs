namespace CN88.Features.Reports.Models
{
    public class WinLossModel
    {
        public WinLossModel()
        {
        }

        public WinLossModel(
            int custId,
            string username,
            string name,
            int wagers,
            decimal stake,
            decimal validStake,
            decimal memberIncome,
            decimal agentCurrency,
            decimal agent,
            decimal agentPoss,
            decimal agentIncome,
            decimal agentResult,
            decimal agentStock,
            decimal master,
            decimal masterPoss,
            decimal masterIncome,
            decimal masterResult,
            decimal masterStock,
            decimal super,
            decimal superPoss,
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

            AgentCurrency = agentCurrency;
            Agent = agent;
            AgentPoss = agentPoss;
            AgentIncome = agentIncome;
            AgentResult = agentResult;
            AgentStock = agentStock;

            Master = master;
            MasterPoss = masterPoss;
            MasterIncome = masterIncome;
            MasterResult = masterResult;
            MasterStock = masterStock;

            Super = super;
            SuperPoss = superPoss;
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

        public int Wagers { get; private set; }

        public decimal Stake { get; private set; }

        public decimal ValidStake { get; private set; }

        public decimal MemberIncome { get; private set; }

        public decimal AgentCurrency { get; private set; }

        public decimal Agent { get; private set; }

        public decimal AgentPoss { get; private set; }

        public decimal AgentIncome { get; private set; }

        public decimal AgentResult { get; private set; }

        public decimal AgentStock { get; private set; }

        public decimal Master { get; private set; }

        public decimal MasterPoss { get; private set; }

        public decimal MasterIncome { get; private set; }

        public decimal MasterResult { get; private set; }

        public decimal MasterStock { get; private set; }

        public decimal Super { get; private set; }

        public decimal SuperPoss { get; private set; }

        public decimal SuperIncome { get; private set; }

        public decimal SuperResult { get; private set; }

        public decimal SuperStock { get; private set; }

        public decimal HeadlinePoss { get; private set; }

        public decimal HeadlineIncome { get; private set; }

        public decimal HeadlineResult { get; private set; }

        public decimal HeadlineStock { get; private set; }

        public WinLossModel Add(WinLossModel winloss)
        {
            if (winloss == null)
            {
                return this;
            }

            Wagers += winloss.Wagers;
            Stake += winloss.Stake;
            ValidStake += winloss.ValidStake;
            MemberIncome += winloss.MemberIncome;

            AgentCurrency += winloss.AgentCurrency;
            Agent += winloss.Agent;
            AgentIncome += winloss.AgentIncome;
            AgentResult += winloss.AgentResult;
            AgentStock += winloss.AgentStock;

            Master += winloss.Master;
            MasterIncome += winloss.MasterIncome;
            MasterResult += winloss.MasterResult;
            MasterStock += winloss.MasterStock;

            Super += winloss.Super;
            SuperIncome += winloss.SuperIncome;
            SuperResult += winloss.SuperResult;
            SuperStock += winloss.SuperStock;

            HeadlineIncome += winloss.HeadlineIncome;
            HeadlineResult += winloss.HeadlineResult;
            HeadlineStock += winloss.HeadlineStock;

            return this;
        }
    }
}