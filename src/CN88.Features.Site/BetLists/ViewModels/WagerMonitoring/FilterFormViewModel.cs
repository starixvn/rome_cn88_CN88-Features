namespace CN88.Features.Site.BetLists.ViewModels.WagerMonitoring
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.BetLists.Constants;
    using Features.BetLists.Resources;
    using Features.Shared.AppResources;
    using Features.Shared.Extensions;
    using Models;
    using Newtonsoft.Json;
    using Requests.WagerMonitoring;

    public class FilterFormViewModel
    {
        public FilterFormViewModel(WagerMonitoringBaseQuery query)
        {
            SportGroup = query.SportGroup;
            Market = query.Market;
            LeagueIds = query.LeagueIds;
            MatchIds = query.MatchIds;
            BetTypeGroupIds = query.BetTypeGroupIds;

            IsSelectAllSports = query.IsSelectAllSports;
            StakeMode = query.StakeMode;
            StakeLimit = query.StakeLimit;
            SportStakeConditions = query.SportStakeConditions;
            StakeLinkText = query.IsSelectAllSports
                ? $"{SharedResources.ResourceManager.GetString($"StakeMode_{query.StakeMode}")} {query.StakeLimit}"
                : string.Empty;
        }

        public int SportGroup { get; }

        public bool IsSelectAllSports { get; }

        public int StakeMode { get; set; }

        public decimal StakeLimit { get; set; }

        public string Market { get; }

        public string LeagueIds { get; }

        public string MatchIds { get; }

        public string BetTypeGroupIds { get; }

        public string StakeLinkText { get; }

        public string SportStakeConditions { get; }

        public string MarketLinkText
        {
            get
            {
                var markets = Market.ToListInt();

                return markets.Any()
                    ? string.Join(
                        ",",
                        WagerMonitoringMarket.Markets
                        .Where(x => markets.Contains(x.Key))
                        .Select(x => SharedResources.ResourceManager.GetString(x.Value)))
                    : SharedResources.AllMarkets;
            }
        }

        public string LeagueLinkText
        {
            get
            {
                var leagueIds = LeagueIds.ToListInt();

                return leagueIds.Any()
                    ? $"{BetListsResources.League} ({leagueIds.Count()})"
                    : BetListsResources.AllLeagues;
            }
        }

        public string EventLinkText
        {
            get
            {
                var matchIds = MatchIds.ToListInt();

                return matchIds.Any()
                    ? $"{BetListsResources.Event} ({matchIds.Count()})"
                    : BetListsResources.AllEvents;
            }
        }

        public string BetTypeLinkText
        {
            get
            {
                var betTypeGroupIds = BetTypeGroupIds.ToListInt();

                return betTypeGroupIds.Any()
                    ? $"{SharedResources.BetType} ({betTypeGroupIds.Count()})"
                    : SharedResources.AllBetTypes;
            }
        }
    }
}