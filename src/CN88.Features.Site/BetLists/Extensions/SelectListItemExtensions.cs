namespace CN88.Features.Site.BetLists.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.BetLists.Models.WagerMonitoring;
    using Features.Shared.AppResources;
    using ViewModels.WagerMonitoring;

    public static class SelectListItemExtensions
    {
        public static SelectListItem Create(KeyValuePair<int, string> resourceId, IEnumerable<int> selectedIds)
        {
            return new SelectListItem
            {
                Value = resourceId.Key.ToString(),
                Text = SharedResources.ResourceManager.GetString(resourceId.Value),
                Selected = selectedIds.IsSelected(resourceId.Key)
            };
        }

        public static SelectListItem Create(League league, IEnumerable<int> selectedIds)
        {
            return new SelectListItem
            {
                Value = league.Id.ToString(),
                Text = league.Name,
                Selected = selectedIds.IsSelected(league.Id)
            };
        }

        public static SelectListItem Create(MatchInfo matchInfo, IEnumerable<int> selectedIds)
        {
            return new SelectListItem
            {
                Value = matchInfo.MatchId.ToString(),
                Text = matchInfo.MatchName,
                Selected = selectedIds.IsSelected(matchInfo.MatchId)
            };
        }

        public static BetTypeSelectListItem Create(MonitorBetTypeGroup betTypeGroup, IEnumerable<int> selectedIds)
        {
            return new BetTypeSelectListItem
            {
                Value = betTypeGroup.DisplayBetTypeGroup.ToString(),
                Text = betTypeGroup.DisplayName,
                IsMainMarket = betTypeGroup.IsMainMarket,
                Selected = selectedIds.IsSelected(betTypeGroup.DisplayBetTypeGroup)
            };
        }

        public static bool IsSelected(this IEnumerable<int> source, int id)
            => !source.Any() || source.Contains(id);
    }
}