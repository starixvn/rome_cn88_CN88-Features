namespace CN88.Features.BetLists.Models
{
    using System;
    using Extensions;

    public class DailyWinLossModel
    {
        public DailyWinLossModel()
        {
        }

        public DailyWinLossModel(
            DateTime winLossDate,
            string custName,
            decimal memberCommission,
            string betId,
            int betType,
            string detail,
            decimal stake,
            decimal winLoss,
            int status,
            decimal agentPercent,
            decimal masterPercent,
            decimal superPercent,
            decimal companyPercent)
        {
            WinLossDate = winLossDate;
            CustName = custName;
            MemberCommission = memberCommission;
            BetId = betId;
            BetType = betType;
            Detail = detail;
            Stake = stake;
            WinLoss = winLoss;
            Status = status;
            AgentPercent = agentPercent;
            MasterPercent = masterPercent;
            SuperPercent = superPercent;
            CompanyPercent = companyPercent;
        }

        public DateTime WinLossDate { get; }

        public string WinLossDateStr => WinLossDate.ToShortDateFormat();

        public string WinLossTimeStr => WinLossDate.ToTimeFormat();

        public string CustName { get; }

        public decimal MemberCommission { get; }

        public string BetId { get; }

        public int BetType { get; }

        public string Detail { get; }

        public decimal Stake { get; }

        public decimal WinLoss { get; }

        public int Status { get; }

        public decimal AgentPercent { get; }

        public decimal MasterPercent { get; }

        public decimal SuperPercent { get; }

        public decimal CompanyPercent { get; }
    }
}
