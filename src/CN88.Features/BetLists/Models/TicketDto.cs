namespace CN88.Features.BetLists.Models
{
    using Fanex.BetList.Core.Entities;

    public class TicketDto : Ticket
    {
        public string UGroup { get; set; }

        public decimal PlayerDiscount { get; set; }
    }
}
