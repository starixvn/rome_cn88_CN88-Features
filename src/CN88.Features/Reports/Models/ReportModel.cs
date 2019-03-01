using System.Collections.Generic;
using System.Linq;
using System.Text;
using CN88.Features.Shared.Extensions;

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
            decimal agentCurrency,
            decimal agent,
            decimal agentIncome,
            decimal agentResult,
            decimal agentStock,
            decimal master,
            decimal masterIncome,
            decimal masterResult,
            decimal masterStock,
            decimal super,
            decimal superIncome,
            decimal superResult,
            decimal superStock,
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
            AgentIncome = agentIncome;
            AgentResult = agentResult;
            AgentStock = agentStock;

            Master = master;
            MasterIncome = masterIncome;
            MasterResult = masterResult;
            MasterStock = masterStock;

            Super = super;
            SuperIncome = superIncome;
            SuperResult = superResult;
            SuperStock = superStock;

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

        public IEnumerable<decimal> AgentPT { get; internal set; } = new List<decimal>();

        public string AgentPoss => GetDisplayPoss(AgentPT);

        public decimal AgentIncome { get; private set; }

        public decimal AgentResult { get; private set; }

        public decimal AgentStock { get; private set; }

        public decimal Master { get; private set; }

        public IEnumerable<decimal> MasterPT { get; internal set; } = new List<decimal>();

        public string MasterPoss => GetDisplayPoss(MasterPT);

        public decimal MasterIncome { get; private set; }

        public decimal MasterResult { get; private set; }

        public decimal MasterStock { get; private set; }

        public decimal Super { get; private set; }

        public IEnumerable<decimal> SuperPT { get; internal set; } = new List<decimal>();

        public string SuperPoss => GetDisplayPoss(SuperPT);

        public decimal SuperIncome { get; private set; }

        public decimal SuperResult { get; private set; }

        public decimal SuperStock { get; private set; }

        public IEnumerable<decimal> ExtraPT { get; internal set; } = new List<decimal>();

        public string HeadlinePoss => GetDisplayPoss(ExtraPT);

        public decimal HeadlineIncome { get; private set; }

        public decimal HeadlineResult { get; private set; }

        public decimal HeadlineStock { get; private set; }

        public ReportModel Add(ReportModel winloss)
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

        private string GetDisplayPoss(IEnumerable<decimal> pts)
        {
            var builder = new StringBuilder();
            var length = pts.Count();

            for (var i = 0; i < length; i++)
            {
                builder.Append(pts.ElementAt(i).FormatNumber());

                if (i != length - 1)
                {
                    builder.Append(" / ");
                }
            }

            return builder.ToString();
        }
    }
}