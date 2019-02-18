using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CN88.Features.Shared.AppResources;
using CN88.Features.Shared.Models;

namespace CN88.Features.Site.Shared
{
    public class BaseHandler
    {
        protected internal IEnumerable<SelectListItem> BuildSportGroupSelectListItems(
            IEnumerable<SportGroup> sportGroups,
            int userSelectedSportGroupId)
        {
            var items = sportGroups.Select(sportGroup => new SelectListItem
            {
                Text = sportGroup.DisplayName,
                Value = sportGroup.Id.ToString(),
                Selected = sportGroup.Id == userSelectedSportGroupId
            }).ToList();

            items.Insert(0, new SelectListItem { Text = SharedResources.AllSports, Value = "0", Selected = userSelectedSportGroupId == 0 });

            return items;
        }

        protected internal IEnumerable<SelectListItem> BuildBetTypeSelectListItems(
            IEnumerable<BetType> betTypes,
            int userSelectedBetTypeId)
        {
            var items = betTypes.Select(betType => new SelectListItem
            {
                Text = betType.DisplayName,
                Value = betType.Id.ToString(),
                Selected = betType.Id == userSelectedBetTypeId
            }).ToList();

            items.Insert(0, new SelectListItem { Text = SharedResources.AllBetTypes, Value = "0", Selected = userSelectedBetTypeId == 0 });

            return items;
        }
    }
}