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
            BreadcrumbLevel = breadcrumbLevel.ToList();
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

        public IEnumerable<BreadcrumbItem> BuildBreadcrumbLinks(string currentLink, bool isSettlement, IEnumerable<string> breadcrumbParamByLevel = null)
        {
            if(BreadcrumbLevel.Any())
            {
                var linkIndex = 0;
                return BreadcrumbLevel.Select(custInfo => 
                    new BreadcrumbItem(custInfo.CustName, BuildBreadcrumbLink(custInfo, currentLink, breadcrumbParamByLevel, linkIndex++, isSettlement)));
            }

            return Enumerable.Empty<BreadcrumbItem>();
        }
    }

    public class BreadcrumbItem
    {
        public BreadcrumbItem(string text, string link)
        {
            Text = text;
            Link = link;
        }

        public string Text { get; }

        public string Link { get; }
    }
}