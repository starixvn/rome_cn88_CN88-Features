namespace CN88.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Shared.Models;
    using Helpers;

    public class BreadcrumbLevelViewModel
    {
        public BreadcrumbLevelViewModel(
            string mainLink,
            IEnumerable<CustInfo> breadcrumbLevel,
            string breadcrumbLevelJson = null)
        {
            MainLink = mainLink;
            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson ?? JsonHelper.ToJsonFunc(breadcrumbLevel);
        }

        public string MainLink { get; }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        public string BuildBreadcrumbLink(CustInfo model, string currentLink, IEnumerable<string> breadcrumbParamByLevel, int breadcrumbIndex, bool isSettlement)
        {
            var url = breadcrumbParamByLevel == null
                ? currentLink
                : UrlHelper.GetSiteUrl(breadcrumbParamByLevel.ElementAt(breadcrumbIndex), currentLink);

            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                url,
                new Dictionary<string, object>
                {
                    ["CustId"] = model.CustId,
                    ["CustName"] = model.CustName,
                    ["CustLevelId"] = model.CustLevelId,
                    ["IsOwner"] = model.IsOwner,
                    ["BreadcrumbLevelJson"] = BreadcrumbLevelJson,
                    ["IsSettlement"] = isSettlement
                });
        }
    }
}