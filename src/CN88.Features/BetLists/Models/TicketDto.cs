namespace CN88.Features.BetLists.Models
{
    using Fanex.BetList.Core.Entities;

    public class TicketDto : Ticket
    {
        public string UGroup { get; set; }

        public decimal PlayerDiscount { get; set; }

        public decimal Payout { get; set; }

        public decimal CompanyPositionTaking()
            => 1 - AgentPositionTaking - MasterPositionTaking - SuperPositionTaking;
    }
}