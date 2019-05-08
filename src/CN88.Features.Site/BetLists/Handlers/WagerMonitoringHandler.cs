namespace CN88.Features.Site.BetLists.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BetList.Core.Builders;
    using Extensions;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Constants;
    using Features.BetLists.Params;
    using Features.Shared.Constants;
    using Features.Shared.Extensions;
    using Features.Shared.Models;
    using MediatR;
    using Models;
    using Newtonsoft.Json;
    using Requests.WagerMonitoring;
    using Shared;
    using ViewModels.WagerMonitoring;

    public class WagerMonitoringHandler :
        BaseHandler,
        IAsyncRequestHandler<WagerMonitoringQuery, WagerMonitoringViewModel>,
        IAsyncRequestHandler<LeagueQuery, MultipleSelectViewModel>,
        IAsyncRequestHandler<EventQuery, MultipleSelectViewModel>
    {
        private readonly ITicketHelper ticketHelper;
        private readonly IBetListCN88HtmlBuilder cn88Builder;
        private readonly IBetListHTMLBuilder builder;
        private readonly WagerMonitoringService wagerMonitoringService;

        public WagerMonitoringHandler(
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            WagerMonitoringService wagerMonitoringService)
        {
            this.builder = builder;
            this.cn88Builder = cn88Builder;
            this.ticketHelper = ticketHelper;
            this.wagerMonitoringService = wagerMonitoringService;
        }

        public async Task<WagerMonitoringViewModel> Handle(WagerMonitoringQuery message)
        {
            var sportGroups = await wagerMonitoringService.GetSportGroups();

            var param = new WagerMonitoringParam(
                message.ViewerId,
                message.ViewerLevelId,
                message.SportGroup,
                FormatStakeConditions(message),
                message.Market,
                message.LeagueIds,
                message.MatchIds,
                message.BetTypeGroupIds,
                message.MinTransId);

            var tickets = await wagerMonitoringService.GetMonitorBets(param);

            var betTypeGroups = await wagerMonitoringService.GetBetTypeGroupsBySport(message.SportGroup);

            return new WagerMonitoringViewModel(
                param.MinTransId,
                message,
                tickets,
                ticketHelper,
                cn88Builder,
                builder,
                BuildSportGroupSelectListItems(sportGroups, message.SportGroup),
                BuildSportStakeConditions(
                    sportGroups,
                    JsonConvert.DeserializeObject<IEnumerable<SportStakeCondition>>(message.SportStakeConditions ?? string.Empty)),
                BuildSelectListItems(WagerMonitoringMarket.Markets, message.Market, SelectListItemExtensions.Create),
                BuildSelectListItems(betTypeGroups, message.BetTypeGroupIds, SelectListItemExtensions.Create));
        }

        public async Task<MultipleSelectViewModel> Handle(LeagueQuery message)
        {
            var leagues = await wagerMonitoringService.GetLeagues(message.SportGroup);

            return new MultipleSelectViewModel(
                BuildSelectListItems(leagues, message.LeagueIds, SelectListItemExtensions.Create),
                string.IsNullOrWhiteSpace(message.LeagueIds));
        }

        public async Task<MultipleSelectViewModel> Handle(EventQuery message)
        {
            var matches = await wagerMonitoringService.GetMatches(message.SportGroup);

            return new MultipleSelectViewModel(
                BuildSelectListItems(matches, message.MatchIds, SelectListItemExtensions.Create),
                string.IsNullOrWhiteSpace(message.MatchIds));
        }

        private IEnumerable<SportStakeCondition> BuildSportStakeConditions(IEnumerable<SportGroup> sportGroups, IEnumerable<SportStakeCondition> sportStakeConditions)
        {
            var conditions = new List<SportStakeCondition>();

            foreach (var sportGroup in sportGroups)
            {
                var condition = sportStakeConditions?.FirstOrDefault(c => c.SportGroupId == sportGroup.Id);
                var stakeMode = condition?.StakeMode ?? StakeModeConstant.GreaterThan;
                var stakeLimit = condition?.StakeLimit ?? 0;

                conditions.Add(new SportStakeCondition
                {
                    SportGroupId = sportGroup.Id,
                    SportGroupName = sportGroup.DisplayName,
                    StakeMode = stakeMode,
                    StakeLimit = stakeLimit
                });
            }

            return conditions;
        }

        private string FormatStakeConditions(WagerMonitoringQuery message)
        {
            if (message.IsSelectAllSports)
            {
                var conditions = new List<SportStakeCondition>();
                conditions.Add(new SportStakeCondition
                {
                    SportGroupId = 0,
                    StakeMode = message.StakeMode,
                    StakeLimit = message.StakeLimit
                });

                return FormatStakeConditions(conditions);
            }

            return FormatStakeConditions(
                JsonConvert.DeserializeObject<IEnumerable<SportStakeCondition>>(message.SportStakeConditions));
        }

        private string FormatStakeConditions(IEnumerable<SportStakeCondition> conditions)
        {
            var builder = new StringBuilder("<ROOT>");
            foreach (var condition in conditions)
            {
                builder.Append($"<SportTypeGroup Id=\"{condition.SportGroupId}\" Condition=\"{condition.StakeMode}\" Stake=\"{condition.StakeLimit}\" />");
            }
            builder.Append("</ROOT>");

            return builder.ToString();
        }

        //public async Task<WagerMonitoringFilterViewModel> Handle(WagerMonitoringFilterQuery message)
        //{
        //    var sportGroups = await wagerMonitoringService.GetSportGroups();
        //    var leagues = await wagerMonitoringService.GetLeagues(message.SportGroup);
        //    var matches = await wagerMonitoringService.GetMatches(message.SportGroup);
        //    var betTypeGroups = await wagerMonitoringService.GetBetTypeGroupsBySport(message.SportGroup);

        //    return new WagerMonitoringFilterViewModel(
        //        message,
        //        BuildSelectListItems(WagerMonitoringMarket.Markets, message.Market, SelectListItemExtensions.Create),
        //        BuildSelectListItems(leagues, message.LeagueIds, SelectListItemExtensions.Create),
        //        BuildSelectListItems(matches, message.MatchIds, SelectListItemExtensions.Create),
        //        BuildSelectListItems(betTypeGroups, message.BetTypeGroupIds, SelectListItemExtensions.Create),
        //        BuildSportStakeConditions(
        //            sportGroups,
        //            JsonConvert.DeserializeObject<IEnumerable<SportStakeCondition>>(message.SportStakeConditions ?? string.Empty)));
        //}

        private IEnumerable<SelectListItem> BuildSelectListItems<T>(
            IEnumerable<T> sources,
            string selectedIds,
            Func<T, IEnumerable<int>, SelectListItem> createItemFunc)
        {
            if (sources == null || !sources.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return sources.Select(item => createItemFunc(item, selectedIds.ToListInt()));
        }

        
    }
}