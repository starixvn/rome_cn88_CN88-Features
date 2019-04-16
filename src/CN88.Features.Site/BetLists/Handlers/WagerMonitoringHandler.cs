namespace CN88.Features.Site.BetLists.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists;
    using Features.BetLists.Constants;
    using Features.BetLists.Models.WagerMonitoring;
    using Features.BetLists.Params;
    using Features.Shared.AppResources;
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
        IAsyncRequestHandler<WagerMonitoringFilterQuery, WagerMonitoringFilterViewModel>
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
                message.BetTypeGroupIds);

            var tickets = await wagerMonitoringService.GetMonitorBets(param);

            return new WagerMonitoringViewModel(
                message,
                tickets,
                BuildSportGroupSelectListItems(sportGroups, message.SportGroup),
                ticketHelper,
                cn88Builder,
                builder);
        }

        private IEnumerable<SportStakeCondition> BuildSportStakeConditions(IEnumerable<SportGroup> sportGroups, IEnumerable<SportStakeCondition> sportStakeConditions)
        {
            var conditions = new List<SportStakeCondition>();

            foreach (var sportGroup in sportGroups)
            {
                var condition = sportStakeConditions?.FirstOrDefault(c => c.SportGroupId == sportGroup.Id);
                var stakeMode = condition != null ? condition.StakeMode : StakeModeConstant.GreaterThan;
                var stakeLimit = condition != null ? condition.StakeLimit : 0;

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
            else
            {
                return FormatStakeConditions(
                    JsonConvert.DeserializeObject<IEnumerable<SportStakeCondition>>(message.SportStakeConditions));
            }
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

        public async Task<WagerMonitoringFilterViewModel> Handle(WagerMonitoringFilterQuery message)
        {
            var sportGroups = await wagerMonitoringService.GetSportGroups();
            var leagues = await wagerMonitoringService.GetLeagues(message.SportGroup);
            var matches = await wagerMonitoringService.GetMatches(message.SportGroup);
            var betTypeGroups = await wagerMonitoringService.GetBetTypeGroupsBySport(message.SportGroup);

            return new WagerMonitoringFilterViewModel(
                message,
                BuildMarketSelectListItems(message.Market.ToListInt()),
                BuildLeagueSelectListItems(leagues, message.LeagueIds.ToListInt()),
                BuildMatchSelectListItems(matches, message.MatchIds.ToListInt()),
                BuildBetTypeGroupSelectListItems(betTypeGroups, message.BetTypeGroupIds.ToListInt()),
                BuildSportStakeConditions(
                    sportGroups,
                    JsonConvert.DeserializeObject<IEnumerable<SportStakeCondition>>(message.SportStakeConditions ?? string.Empty)));
        }

        private IEnumerable<SelectListItem> BuildMarketSelectListItems(IEnumerable<int> selectedMarkets)
            => WagerMonitoringMarket.Markets.Select(
                market => new SelectListItem
                {
                    Value = market.Key.ToString(),
                    Text = SharedResources.ResourceManager.GetString(market.Value),
                    Selected = !selectedMarkets.Any() || selectedMarkets.Contains(market.Key)
                });

        private IEnumerable<SelectListItem> BuildLeagueSelectListItems(IEnumerable<League> leagues, IEnumerable<int> selectedLeagueIds)
        {
            if (leagues == null || !leagues.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return leagues.Select(league => new SelectListItem
            {
                Value = league.Id.ToString(),
                Text = league.Name,
                Selected = !selectedLeagueIds.Any() || selectedLeagueIds.Contains(league.Id)
            });
        }

        private IEnumerable<SelectListItem> BuildMatchSelectListItems(IEnumerable<MatchInfo> matches, IEnumerable<int> selectedMatchIds)
        {
            if (matches == null || !matches.Any())
            {
                return Enumerable.Empty<SelectListItem>();
            }

            return matches.Select(match => new SelectListItem
            {
                Value = match.MatchId.ToString(),
                Text = match.MatchName,
                Selected = !selectedMatchIds.Any() || selectedMatchIds.Contains(match.MatchId)
            });
        }

        private IEnumerable<BetTypeSelectListItem> BuildBetTypeGroupSelectListItems(IEnumerable<MonitorBetTypeGroup> betTypeGroups, IEnumerable<int> selectedBetTypeGroupIds)
        {
            if (betTypeGroups == null || !betTypeGroups.Any())
            {
                return Enumerable.Empty<BetTypeSelectListItem>();
            }

            return betTypeGroups.Select(group => new BetTypeSelectListItem
            {
                Value = group.DisplayBetTypeGroup.ToString(),
                Text = group.DisplayName,
                IsMainMarket = group.IsMainMarket,
                Selected = !selectedBetTypeGroupIds.Any() || selectedBetTypeGroupIds.Contains(group.DisplayBetTypeGroup)
            });
        }
    }
}