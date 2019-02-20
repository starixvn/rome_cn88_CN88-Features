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
            decimal? agentCurrency,
            NetWinLoss headlineNetWinLoss,
            NetWinLoss superNetWinLoss,
            NetWinLoss masterNetWinLoss,
            NetWinLoss agentNetWinLoss)
        {
            CustId = custId;
            Username = username;
            Name = name;
            Wagers = wagers;
            Stake = stake;
            ValidStake = validStake;
            MemberIncome = memberIncome;
            AgentCurrency = agentCurrency;

            HeadlineNetWinLoss = headlineNetWinLoss;
            SuperNetWinLoss = superNetWinLoss;
            MasterNetWinLoss = masterNetWinLoss;
            AgentNetWinLoss = agentNetWinLoss;
        }

        public int CustId { get; }

        public string Username { get; }

        public string Name { get; }

        public int Wagers { get; private set; }

        public decimal Stake { get; private set; }

        public decimal ValidStake { get; private set; }

        public decimal MemberIncome { get; private set; }

        public decimal? AgentCurrency { get; private set; }

        public NetWinLoss HeadlineNetWinLoss { get; }

        public NetWinLoss SuperNetWinLoss { get; }

        public NetWinLoss MasterNetWinLoss { get; }

        public NetWinLoss AgentNetWinLoss { get; }

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

            return this;
        }
    }
}