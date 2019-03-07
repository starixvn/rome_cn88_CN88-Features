namespace CN88.Features.Site.BetLists.Handlers
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
    using Features.Reports.Services;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using Shared;
    using ViewModels;

    public class BetListsHandler :
        BaseHandler,
        IAsyncRequestHandler<BetListOfReportQuery, BetListOfReportViewModel>,
        IAsyncRequestHandler<GetParlayDetailQuery, ParlayDetailViewModel>,
        IAsyncRequestHandler<GetParlaySubTicketsQuery, ParlaySubTicketsViewModel>
    {
        private BetListsService service;
        private ISmartPunterBetListMonitor monitorService;
        private ITicketHelper ticketHelper;
        private IBetListCN88HtmlBuilder cn88Builder;
        private IBetListHTMLBuilder builder;

        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly AccountingPeriodService accountingPeriodService;

        public BetListsHandler(
            BetListsService service,
            ISmartPunterBetListMonitor monitorService,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            AccountingPeriodService accountingPeriodService)
        {
            this.service = service;
            this.monitorService = monitorService;
            this.ticketHelper = ticketHelper;
            this.cn88Builder = cn88Builder;
            this.builder = builder;

            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.accountingPeriodService = accountingPeriodService;
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

            var sportGroups = sportGroupService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport();
            var currentPeriod = accountingPeriodService.GetCurrentPeriod();
            var lastPeriod = accountingPeriodService.GetLastPeriod();

            return new BetListOfReportViewModel(
                message,
                betlistData,
                ticketHelper,
                cn88Builder,
                builder,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0),
                currentPeriod,
                lastPeriod);
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

        public async Task<ParlaySubTicketsViewModel> Handle(GetParlaySubTicketsQuery message)
        {
            var param = new GetParlaySubTicketsParam(message.TransId, message.CustId);

            var tickets = await service.GetParlaySubTickets(param);

            return new ParlaySubTicketsViewModel(message, new BetListData(tickets, null), cn88Builder, builder, ticketHelper);
        }

        protected internal void WriteLog(int custId, IEnumerable<TicketDto> tickets)
             => monitorService.WriteLog(custId, tickets.ToList<ITicket>());
    }
}