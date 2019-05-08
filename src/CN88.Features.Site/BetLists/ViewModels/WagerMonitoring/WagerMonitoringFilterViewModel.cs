namespace CN88.Features.Site.BetLists.ViewModels.WagerMonitoring
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Models;
    using Requests.WagerMonitoring;

    public class WagerMonitoringFilterViewModel
    {
        public WagerMonitoringFilterViewModel(
            WagerMonitoringQuery query,
            IEnumerable<SportStakeCondition> sportStakeConditions,
            IEnumerable<SelectListItem> markets,
            IEnumerable<SelectListItem> betTypeGroups)
        {
            SportGroup = query.SportGroup;

            SelectedAllMarkets = string.IsNullOrWhiteSpace(query.Market);
            SelectedAllLeagues = string.IsNullOrWhiteSpace(query.LeagueIds);
            SelectedAllMatches = string.IsNullOrWhiteSpace(query.MatchIds);
            SelectedAllBetTypes = string.IsNullOrWhiteSpace(query.BetTypeGroupIds);

            Markets = markets;
            BetTypeGroups = betTypeGroups.Select(x => (BetTypeSelectListItem)x);
            SportStakeConditionViewModel = new SportStakeConditionViewModel(
                query.IsSelectAllSports,
                query.StakeMode,
                query.StakeLimit,
                sportStakeConditions);
        }

        public int SportGroup { get; }

        public bool SelectedAllMarkets { get; }

        public bool SelectedAllLeagues { get; }

        public bool SelectedAllMatches { get; }

        public bool SelectedAllBetTypes { get; }

        public IEnumerable<SelectListItem> Markets { get; }

        public IEnumerable<BetTypeSelectListItem> BetTypeGroups { get; }

        public IEnumerable<BetTypeSelectListItem> MainBetTypeGroups => BetTypeGroups.Where(x => x.IsMainMarket);

        public bool IsSelectedAllMainBetTypeGroups => !MainBetTypeGroups.Any(x => !x.Selected);

        public IEnumerable<BetTypeSelectListItem> OtherBetTypeGroups => BetTypeGroups.Where(x => !x.IsMainMarket);

        public bool IsSelectedAllOtherBetTypeGroups => !OtherBetTypeGroups.Any(x => !x.Selected);

        public SportStakeConditionViewModel SportStakeConditionViewModel { get; }
    }
}