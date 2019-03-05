namespace CN88.Features.BetLists.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class TicketDetailModel
    {
        public TicketDetailModel(IEnumerable<TicketDto> tickets)
        {
            Tickets = tickets ?? Enumerable.Empty<TicketDto>();
        }

        public IEnumerable<TicketDto> Tickets { get; }
    }
}
