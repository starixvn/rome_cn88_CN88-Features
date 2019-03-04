namespace CN88.Features.Site.BetLists.ViewModels
{
    using Features.BetLists.Models;
    using Features.Shared.Models;
    using Requests;

    public class BetListOfReportViewModel
    {
        public BetListOfReportViewModel(
            BetListOfReportQuery message,
            BetListData betListData,
            string[] excludingTicketStatus = null)
        {
            CustLevel = message.CustLevel;
            NoOfColumns = CustLevel.IsSuperLevel() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, excludingTicketStatus);
        }

        public UserLevel CustLevel { get; }

        public int NoOfColumns { get; }

        public BetListData BetListData { get; protected set; }

        public BetListTotal BetListTotal { get; }
    }
}