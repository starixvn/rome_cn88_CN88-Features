namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
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
        IRequestHandler<BetListOfReportQuery, BetListOfReportViewModel>,
        IRequestHandler<BetListOfStatementQuery, BetListOfStatementViewModel>,
        IRequestHandler<GetParlayDetailQuery, ParlayDetailViewModel>,
        IRequestHandler<GetParlaySubTicketsQuery, ParlaySubTicketsViewModel>
    {
        private readonly BetListsService service;
        private readonly ISmartPunterBetListMonitor monitorService;
        private readonly ITicketHelper ticketHelper;
        private readonly IBetListCN88HtmlBuilder cn88Builder;
        private readonly IBetListHTMLBuilder builder;

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

        public BetListOfReportViewModel Handle(BetListOfReportQuery message)
        {
            var param = new BetListOfReportParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.SportTypeGroup,
                message.BetTypeGroup,
                message.LiveIndicator,
                message.Type);

            var betlistData = service.GetBetListOfReport(param);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            var sportGroups = sportGroupService.GetSportGroups();
            var betTypes = betTypeService.GetBetTypeGroupsBySport();
            var currentPeriod = accountingPeriodService.GetCurrentPeriod();
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            var allBetTypes = betTypeService.GetBetTypeGroupsBySport(0);

            return new BetListOfReportViewModel(
                message,
                betlistData,
                ticketHelper,
                cn88Builder,
                builder,
                BuildSportGroupSelectListItems(sportGroups, 0),
                BuildBetTypeSelectListItems(betTypes, 0),
                currentPeriod,
                lastPeriod,
                BuildBetTypeSelectListItems(allBetTypes, 0));
        }

        public BetListOfStatementViewModel Handle(BetListOfStatementQuery message)
        {
            var param = new BetListOfStatementParam(
                message.CustId,
                message.Date,
                message.SportTypeGroup);

            var betlistData = service.GetBetListOfStatement(param);

            WriteLog(message.CustId == 0 ? message.ViewerId : message.CustId, betlistData.Tickets);

            return new BetListOfStatementViewModel(
                message,
                betlistData,
                ticketHelper,
                cn88Builder,
                builder);
        }

        public ParlayDetailViewModel Handle(GetParlayDetailQuery message)
        {
            var param = new GetParlayDetailParam(
                message.RefNo,
                message.TransId,
                message.BetId,
                message.CustId);

            var parlayDetailData = service.GetParlayDetails(param);

            return new ParlayDetailViewModel(message, parlayDetailData, ticketHelper, isExportingExcel: false);
        }

        public ParlaySubTicketsViewModel Handle(GetParlaySubTicketsQuery message)
        {
            var param = new GetParlaySubTicketsParam(message.TransId, message.CustId);

            var tickets = service.GetParlaySubTickets(param);

            return new ParlaySubTicketsViewModel(message, new BetListData(tickets, null), cn88Builder, builder, ticketHelper);
        }

        protected internal void WriteLog(int custId, IEnumerable<TicketDto> tickets)
             => monitorService.WriteLog(custId, tickets.ToList<ITicket>());
    }
}