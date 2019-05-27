namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using CN88.Web.Helpers;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Constants;
    using Features.BetLists.Models;
    using Features.BetLists.Params;
    using Features.BetLists.Resources;
    using Features.Reports.Services;
    using Features.Shared.Constants;
    using Features.Shared.Services;
    using MediatR;
    using Requests;
    using Shared;
    using Shared.ViewModels;
    using ViewModels;

    public class BetListsHandler :
        BaseHandler,
        IRequestHandler<BetListOfReportQuery, BetListOfReportViewModel>,
        IRequestHandler<BetListOfStatementQuery, BetListOfStatementViewModel>,
        IRequestHandler<GetParlayDetailQuery, ParlayDetailViewModel>,
        IRequestHandler<GetParlaySubTicketsQuery, ParlaySubTicketsViewModel>,
        IRequestHandler<OutrightWagerByAccountQuery, OutrightWagerByAccountViewModel>,
        IRequestHandler<OutrightWagerDetailsQuery, OutrightWagerDetailsViewModel>,
        IRequestHandler<EventWagerByAccountQuery, EventWagerByAccountViewModel>,
        IRequestHandler<EventWagerDetailsQuery, EventWagerDetailsViewModel>,
        IRequestHandler<TotalBetParlayQuery, TotalBetParlayViewModel>,
        IRequestHandler<StartedWagerByAccountQuery, StartedWagerByAccountViewModel>,
        IRequestHandler<StartedWagerDetailsQuery, StartedWagerDetailsViewModel>
    {
        private readonly BetListsService betListService;
        private readonly ITicketHelper ticketHelper;
        private readonly IBetListCN88HtmlBuilder cn88Builder;
        private readonly IBetListHTMLBuilder builder;

        private readonly SportGroupService sportGroupService;
        private readonly BettypeGroupService betTypeService;
        private readonly AccountingPeriodService accountingPeriodService;

        private readonly BrowserHelper browserHelper;

        public BetListsHandler(
            BetListsService service,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            SportGroupService sportGroupService,
            BettypeGroupService betTypeService,
            AccountingPeriodService accountingPeriodService,
            BrowserHelper browserHelper)
        {
            this.betListService = service;
            this.ticketHelper = ticketHelper;
            this.cn88Builder = cn88Builder;
            this.builder = builder;

            this.sportGroupService = sportGroupService;
            this.betTypeService = betTypeService;
            this.accountingPeriodService = accountingPeriodService;

            this.browserHelper = browserHelper;
        }

        public BetListOfReportViewModel Handle(BetListOfReportQuery message)
        {
            var param = new BetListOfReportParam(
                message.CustId,
                message.FromDate,
                message.ToDate,
                message.UserSelectedSportGroupId,
                message.UserSelectedBetTypeId,
                message.UserSelectedLiveStatusId,
                message.Type,
                message.LimitedDateToShowInfo);

            var betlistData = betListService.GetBetListOfReport(param);

            var sportGroups = sportGroupService.GetSportGroups();
            var allBetTypes = betTypeService.GetBetTypeGroups();
            var betTypes = allBetTypes.Where(betType => betType.SportTypeGroup == message.UserSelectedSportGroupId);
            var lastPeriod = accountingPeriodService.GetLastPeriod();
            var oldAccountingPeriods = accountingPeriodService.GetOldAccountingPeriods();

            return new BetListOfReportViewModel(
                message,
                betlistData,
                ticketHelper,
                cn88Builder,
                builder,
                BuildSportGroupSelectListItems(sportGroups, message.UserSelectedSportGroupId),
                BuildBetTypeSelectListItems(betTypes, message.UserSelectedBetTypeId),
                lastPeriod,
                BuildBetTypeSelectListItems(allBetTypes, 0),
                oldAccountingPeriods,
                browserHelper.IsMobile());
        }

        public BetListOfStatementViewModel Handle(BetListOfStatementQuery message)
        {
            var param = new BetListOfStatementParam(
                message.CustId,
                message.Date,
                message.SportTypeGroup);

            var betlistData = betListService.GetBetListOfStatement(param);

            return new BetListOfStatementViewModel(
                message,
                betlistData,
                ticketHelper,
                cn88Builder,
                builder,
                browserHelper.IsMobile());
        }

        public ParlayDetailViewModel Handle(GetParlayDetailQuery message)
        {
            var param = new GetParlayDetailParam(
                message.RefNo,
                message.TransId,
                message.BetId,
                message.CustId);

            var parlayDetailData = betListService.GetParlayDetails(param);

            return new ParlayDetailViewModel(message, parlayDetailData, ticketHelper, isExportingExcel: false);
        }

        public ParlaySubTicketsViewModel Handle(GetParlaySubTicketsQuery message)
        {
            var param = new GetParlaySubTicketsParam(message.TransId, message.CustId);

            var tickets = betListService.GetParlaySubTickets(param);

            return new ParlaySubTicketsViewModel(message, new BetListData(tickets, null), cn88Builder, builder, ticketHelper);
        }

        public OutrightWagerByAccountViewModel Handle(OutrightWagerByAccountQuery message)
        {
            var param = new OutrightWagerByAccountParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.CustLevelId,
                message.MatchId,
                message.StakeCondition,
                message.Stake);

            var betlistData = betListService.GetOutrightWagerByAccount(param);

            return new OutrightWagerByAccountViewModel(message, betlistData, ticketHelper, cn88Builder, builder, browserHelper.IsMobile());
        }

        public OutrightWagerDetailsViewModel Handle(OutrightWagerDetailsQuery message)
        {
            var param = new OutrightWagerDetailsParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.ViewerLevelId,
                message.MatchId,
                message.StakeCondition,
                message.Stake);

            var betlistData = betListService.GetOutrightWagerDetails(param);

            return new OutrightWagerDetailsViewModel(message, betlistData, ticketHelper, cn88Builder, builder, browserHelper.IsMobile());
        }

        public EventWagerByAccountViewModel Handle(EventWagerByAccountQuery message)
        {
            var param = new EventWagerByAccountParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.CustLevelId,
                message.MatchId,
                message.BetType,
                message.LiveIndicator,
                message.BetTeam,
                message.StakeCondition,
                message.Stake);

            var betlistData = betListService.GetEventWagerByAccount(param);

            return new EventWagerByAccountViewModel(message, betlistData, ticketHelper, cn88Builder, builder, browserHelper.IsMobile());
        }

        public EventWagerDetailsViewModel Handle(EventWagerDetailsQuery message)
        {
            var param = new EventWagerDetailsParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.ViewerLevelId,
                message.MatchId,
                message.BetType,
                message.LiveIndicator,
                message.BetTeam,
                message.StakeCondition,
                message.Stake);

            var betlistData = betListService.GetEventWagerDetails(param);

            return new EventWagerDetailsViewModel(message, betlistData, ticketHelper, cn88Builder, builder, browserHelper.IsMobile());
        }

        public StartedWagerByAccountViewModel Handle(StartedWagerByAccountQuery message)
        {
            var param = new StartedWagerByAccountParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.CustLevelId,
                message.MatchId,
                message.BetType,
                message.LiveIndicator,
                message.BetTeam,
                message.StakeCondition,
                message.Stake);

            var betlistData = betListService.GetStartedWagerByAccount(param);

            return new StartedWagerByAccountViewModel(message, betlistData, ticketHelper, cn88Builder, builder, browserHelper.IsMobile());
        }

        public StartedWagerDetailsViewModel Handle(StartedWagerDetailsQuery message)
        {
            var param = new StartedWagerDetailsParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.ViewerLevelId,
                message.MatchId,
                message.BetType,
                message.LiveIndicator,
                message.BetTeam,
                message.StakeCondition,
                message.Stake);

            var betlistData = betListService.GetStartedWagerDetails(param);

            return new StartedWagerDetailsViewModel(message, betlistData, ticketHelper, cn88Builder, builder, browserHelper.IsMobile());
        }

        public TotalBetParlayViewModel Handle(TotalBetParlayQuery message)
        {
            var param = new GetTotalBetParlayParam(
                message.CustId,
                message.LimitedDateToShowInfo,
                message.CustLevelId,
                message.SportGroupId,
                message.ViewMode,
                message.StakeMode,
                message.StakeLimit);

            var betlistData = betListService.GetTotalBetParlay(param);

            return new TotalBetParlayViewModel(
                message,
                betlistData,
                ticketHelper,
                cn88Builder,
                builder,
                BuildSportsSelectListItems(TotalBetParlaySport.Sports, message.SportGroupId),
                BuildStakeModeSelectListItems(StakeModeConstant.Modes, message.StakeMode),
                BuildPercentageViewModeSelectListItems(PercentageViewModeConstant.ModesByLevel[message.ActorLevel], message.ViewMode),
                browserHelper.IsMobile());
        }

        protected internal IEnumerable<SelectListItemExt> BuildSportsSelectListItems(IEnumerable<int> sports, int userSelectedSport)
            => sports.Select(sport => new SelectListItemExt
            {
                Text = BetListsResources.ResourceManager.GetString($"TotalBetParlaySport_{sport}"),
                Value = sport.ToString(),
                Selected = sport == userSelectedSport
            });
    }
}