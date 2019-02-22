namespace CN88.Features.Site.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Models;
    using Requests;

    public class BaseHandler
    {
        protected internal IEnumerable<CustInfo> BuildBreadcrumbLevel(BaseQuery query)
        {
            var breadcrumbLevel = new List<CustInfo>();

            if (query.BreadcrumbLevel != null
                && query.BreadcrumbLevel.Any())
            {
                foreach (var custInfo in query.BreadcrumbLevel)
                {
                    if (custInfo.CustLevelId == query.CustLevelId)
                    {
                        break;
                    }

                    breadcrumbLevel.Add(custInfo);
                }
            }

            breadcrumbLevel.Add(
                new CustInfo(
                    query.CustId,
                    query.CustName,
                    query.CustLevel.GetDirectUpLineLevel().Value,
                    query.CustId == query.ViewerId));

            return breadcrumbLevel;
        }

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