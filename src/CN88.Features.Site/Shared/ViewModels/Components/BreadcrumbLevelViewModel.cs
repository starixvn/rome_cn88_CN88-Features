namespace CN88.Features.Site.Shared.ViewModels.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using Features.Reports.Models;
    using Shared.Helpers;

    public class BreadcrumbLevelViewModel
    {
        public BreadcrumbLevelViewModel(
            IEnumerable<CustInfo> breadcrumbLevel,
            string breadcrumbLevelJson = null)
        {
            BreadcrumbLevel = breadcrumbLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson ?? JsonHelper.ToJsonFunc(breadcrumbLevel);
        }

        public IEnumerable<CustInfo> BreadcrumbLevel { get; }

        public string BreadcrumbLevelJson { get; }

        public string BuildBreadcrumbLink(CustInfo model, string currentLink, IEnumerable<string> breadcrumbParamByLevel, int breadcrumbIndex)
        {
            var url = breadcrumbParamByLevel == null
                ? currentLink
                : UrlHelper.GetSiteUrl(breadcrumbParamByLevel.ElementAt(breadcrumbIndex), currentLink);

            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                url,
                new Dictionary<string, object>
                {
                    { "CustId", model.CustId },
                    { "CustName", model.CustName },
                    { "CustLevelId", model.CustLevelId },
                    { "BreadcrumbLevelJson", BreadcrumbLevelJson }
                });
        }
    }
}