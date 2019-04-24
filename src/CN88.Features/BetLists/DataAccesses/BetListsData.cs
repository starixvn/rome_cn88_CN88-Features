namespace CN88.Features.BetLists.DataAccesses
{
    using System.Collections.Generic;
    using Models;
    using Params;

    public static class BetListsData
    {
        public static List<TicketDto> GetMonitorBets(WagerMonitoringParam param)
        {
            var tickets = new List<TicketDto>
            {
                new TicketDto { TransId = 1 },
                new TicketDto { TransId = 2 },
                new TicketDto { TransId = 3 },
                new TicketDto { TransId = 4 },
                new TicketDto { TransId = 5 }
            };

            return tickets;
        }
    }
}
