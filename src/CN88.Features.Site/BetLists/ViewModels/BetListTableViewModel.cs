namespace CN88.Features.Site.BetLists.ViewModels
{
    using CN88.BetList.Core.Builders;
    using CN88.Features.BetLists.Models;
    using CN88.Features.Shared.Models;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class BetListTableViewModel
    {
        public UserLevel CustLevel { get; protected set; }

        public string CustName { get; protected set; } 

        public int NoOfColumns { get; protected set; } 

        public BetListData BetListData { get; protected set; } 

        public BetListTotal BetListTotal { get; protected set; } 

        public ITicketHelper TicketHelper { get; protected set; } 

        public IBetListHTMLBuilder Builder { get; protected set; } 

        public IBetListCN88HtmlBuilder CN88Builder { get; protected set; } 

        public bool DisplayVNIP { get; protected set; }

        public int Divisor { get; protected set; }

        public bool IsMobile { get; protected set; }

        public decimal CompanyPositionTaking(TicketDto ticket)
            => 1 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;
    }
}