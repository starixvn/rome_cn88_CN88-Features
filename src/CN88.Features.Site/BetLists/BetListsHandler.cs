namespace CN88.Features.Site.BetLists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.SmartPunter.Services;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Models;
    using Features.BetLists.Params;
    using MediatR;
    using Requests;
    using ViewModels;

    public class BetListsHandler :
         IAsyncRequestHandler<BetListOfReportQuery, BetListOfReportViewModel>
    {
        private BetListsService service;
        private ISmartPunterBetListMonitor monitorService;

        public BetListsHandler(
            BetListsService service,
            ISmartPunterBetListMonitor monitorService)
        {
            this.service = service;
            this.monitorService = monitorService;
        }

        public async Task<BetListOfReportViewModel> Handle(BetListOfReportQuery message)
        {
            var param = new BetListOfReportParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.SportTypeGroup,
                message.BetTypeGroup,
                message.LiveIndicator,
                message.Type);

            var betlistData = await service.GetBetListOfReport(param);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new BetListOfReportViewModel(message, betlistData);
        }

        protected internal void WriteLog(int custId, IEnumerable<TicketDto> tickets)
             => monitorService.WriteLog(custId, tickets.ToList<ITicket>());
    }
}