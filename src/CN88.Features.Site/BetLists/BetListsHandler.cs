namespace CN88.Features.Site.BetLists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.SmartPunter.Services;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Models;
    using Features.BetLists.Params;
    using MediatR;
    using Requests;
    using ViewModels;

    public class BetListsHandler :
         IAsyncRequestHandler<BetListOfReportQuery, BetListOfReportViewModel>,
         IAsyncRequestHandler<GetParlayDetailQuery, ParlayDetailViewModel>,
         IAsyncRequestHandler<GetSubTicketsQuery, SubTicketsViewModel>
    {
        private BetListsService service;
        private ISmartPunterBetListMonitor monitorService;
        private ITicketHelper ticketHelper;
        private IBetListCN88HtmlBuilder cn88Builder;
        private IBetListHTMLBuilder builder;

        public BetListsHandler(
            BetListsService service,
            ISmartPunterBetListMonitor monitorService,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder)
        {
            this.service = service;
            this.monitorService = monitorService;
            this.ticketHelper = ticketHelper;
            this.cn88Builder = cn88Builder;
            this.builder = builder;
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

            return new BetListOfReportViewModel(message, betlistData, ticketHelper, cn88Builder, builder);
        }

        public async Task<ParlayDetailViewModel> Handle(GetParlayDetailQuery message)
        {
            var param = new GetParlayDetailParam(
                message.RefNo,
                message.TransId,
                message.BetId,
                message.CustId);

            var parlayDetailData = await service.GetParlayDetails(param);

            return new ParlayDetailViewModel(message, parlayDetailData, ticketHelper, isExportingExcel: false);
        }

        public async Task<SubTicketsViewModel> Handle(GetSubTicketsQuery message)
        {
            var param = new GetSubTicketsParam(message.TransId, message.CustId);

            var betlistData = await service.GetSubTickets(param);

            // WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new SubTicketsViewModel(message, betlistData, ticketHelper, cn88Builder, builder);
        }

        protected internal void WriteLog(int custId, IEnumerable<TicketDto> tickets)
             => monitorService.WriteLog(custId, tickets.ToList<ITicket>());
    }
}