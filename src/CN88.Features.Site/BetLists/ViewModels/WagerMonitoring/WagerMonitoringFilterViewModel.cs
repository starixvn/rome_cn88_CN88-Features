namespace CN88.Features.Site.BetLists.ViewModels.WagerMonitoring
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Models;
    using Requests.WagerMonitoring;

    public class WagerMonitoringFilterViewModel
    {
        public WagerMonitoringFilterViewModel(
            WagerMonitoringFilterQuery query,
            IEnumerable<SelectListItem> markets,
            IEnumerable<SelectListItem> leagues,
            IEnumerable<SelectListItem> matches,
            IEnumerable<BetTypeSelectListItem> betTypeGroups,
            IEnumerable<SportStakeCondition> sportStakeConditions)
        {
            SportGroup = query.SportGroup;

            SelectedAllMarkets = string.IsNullOrWhiteSpace(query.Market);
            SelectedAllLeagues = string.IsNullOrWhiteSpace(query.LeagueIds);
            SelectedAllMatches = string.IsNullOrWhiteSpace(query.MatchIds);
            SelectedAllBetTypes = string.IsNullOrWhiteSpace(query.BetTypeGroupIds);

            Markets = markets;
            Leagues = leagues;
            Matches = matches;
            BetTypeGroups = betTypeGroups;

            SportStakeConditionViewModel = new SportStakeConditionViewModel(query.IsSelectAllSports, query.StakeMode, query.StakeLimit, sportStakeConditions);
        }

        public int SportGroup { get; }

        public bool SelectedAllMarkets { get; }

        public bool SelectedAllLeagues { get; }

        public bool SelectedAllMatches { get; }

        public bool SelectedAllBetTypes { get; }

        public IEnumerable<SelectListItem> Markets { get; }

        public IEnumerable<SelectListItem> Leagues { get; }

        public IEnumerable<SelectListItem> Matches { get; }

        public IEnumerable<BetTypeSelectListItem> BetTypeGroups { get; }

        public SportStakeConditionViewModel SportStakeConditionViewModel { get; }
    }
}