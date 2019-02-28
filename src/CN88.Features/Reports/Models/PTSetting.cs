namespace CN88.Features.Reports.Models
{
    public class PTSetting
    {
        public PTSetting()
        {
        }

        public PTSetting(
            int custId,
            int sportType,
            int bettype,
            decimal superPT,
            decimal masterPT,
            decimal agentPT,
            decimal extraPT)
        {
            CustId = custId;
            SportType = sportType;
            BetType = bettype;
            SuperPT = superPT;
            MasterPT = masterPT;
            AgentPT = agentPT;
            ExtraPT = extraPT;
        }

        public int CustId { get; }

        public int SportType { get; }

        public int BetType { get; }

        public decimal SuperPT { get; }

        public decimal MasterPT { get; }

        public decimal AgentPT { get; }

        public decimal ExtraPT { get; }
    }
}