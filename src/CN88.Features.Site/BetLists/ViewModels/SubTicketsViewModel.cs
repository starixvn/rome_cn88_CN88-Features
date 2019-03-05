namespace CN88.Features.Site.BetLists.ViewModels
{
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Models;
    using Features.Shared.Models;
    using Requests;

    public class SubTicketsViewModel
    {
        public SubTicketsViewModel(
           GetSubTicketsQuery message,
           BetListData betListData,
           ITicketHelper ticketHelper,
           IBetListCN88HtmlBuilder cn88Builder,
           IBetListHTMLBuilder builder,
           string[] excludingTicketStatus = null)
        {
            CustLevel = message.CustLevel;
            DisplayVNIP = message.DisplayVNIP;

            NoOfColumns = CustLevel.IsSuperLevel() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;
            CustId = message.CustId;
        }

        public UserLevel CustLevel { get; }

        public int NoOfColumns { get; }

        public BetListData BetListData { get; }

        public BetListTotal BetListTotal { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListHTMLBuilder Builder { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public bool DisplayVNIP { get; }

        public int CustId { get; }

        public decimal CompanyPositionTaking(TicketDto ticket)
        {
            return 100 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;
        }

    }
}