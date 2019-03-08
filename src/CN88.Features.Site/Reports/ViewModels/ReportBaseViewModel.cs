namespace CN88.Features.Site.Reports.ViewModels
{
    using Alpha.Web.Helpers;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;
    using Sunplus.Agent.Model.BL;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public abstract class ReportBaseViewModel
    {
        protected readonly ReportBaseQuery query;

        protected ReportBaseViewModel(
            ReportBaseQuery query,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal)
        {
            this.query = query;

            ReportData = reportData;
            GrandTotal = grandTotal;           
            Divisor = (query.ActorLevel.IsHeadline()) ? 100 : 1;
        }

        public bool IsNotOwner => !query.IsOwner;

        public UserLevel ReportLevel => query.CustLevel;

        public int Divisor { get; }

        public IEnumerable<ReportModel> ReportData { get; }

        public ReportModel GrandTotal { get; }

        public decimal ProfitMargin
            => query.CustLevel.IsSuperLevel() && GrandTotal.SuperStock != 0
                ? GrandTotal.SuperResult / GrandTotal.SuperStock * 100
                : 0;

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel
            => new BreadcrumbLevelViewModel(BuildReportMainUrl(), GetBreadcrumbLevel());

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(query.PageIndex, query.PageSize, ReportData.Count());

        public string Title => ReportResources.ResourceManager.GetString(ReportTypeConstants.ReportTypes[query.ReportType]);

        public string BuildDrillDownLink(ReportModel model, string currentLink)
            => ReportLevel.IsMemberLevel()
            ? BuildBetListLink(model)
            : BuildDownlineLink(model, currentLink);

        protected abstract string GetReportMainActionPath();

        protected abstract int GetBetListType(int reportType);
        
        private string BuildBetListLink(ReportModel model)
           => UrlHelper.AddQueryStringParamsFunc(
               AppPathHelper.GetAppPath(Features.Shared.Configurations.AppSettings.FeaturesAppPath, "~/BetLists/BetListOfReport"),
               GetBetListLinkRouteValues(model));

        private string BuildDownlineLink(ReportModel model, string currentLink)
        {
            var routeValues = GetDownlineLinkRouteValues(model);
            return UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentLink,
                new Dictionary<string, object>(routeValues)
                {
                    ["CustLevelId"] = ReportLevel.GetDirectDownLineLevel().Value,
                    ["BreadcrumbLevelJson"] = JsonHelper.ToJsonFunc(BuildBreadcrumbLevel(model))
                });
        }

        private IDictionary<string, object> GetDownlineLinkRouteValues(ReportModel model)
            => new Dictionary<string, object>
            {
                ["CustId"] = model.CustId,
                ["CustName"] = model.Username,
                ["FromDate"] = query.FromDate,
                ["ToDate"] = query.ToDate,
                ["IsOwner"] = false,
                ["ReportType"] = query.ReportType,
                ["UserSelectedSportGroupId"] = query.UserSelectedSportGroupId,
                ["UserSelectedBetTypeId"] = query.UserSelectedBetTypeId,
                ["UserSelectedLiveStatusId"] = query.UserSelectedLiveStatusId,
                ["BreadcrumbLevelJson"] = JsonHelper.ToJsonFunc(BuildBreadcrumbLevel(model))
            };

        private IEnumerable<CustInfo> BuildBreadcrumbLevel(ReportModel model)
        {
            var breadcrumbLevel = new List<CustInfo>();

            breadcrumbLevel.AddRange(GetBreadcrumbLevel());

            breadcrumbLevel.Add(
                new CustInfo(
                    query.CustId,
                    model.Username,
                    query.CustLevelId,
                    query.IsOwner));

            return breadcrumbLevel;
        }

        private IEnumerable<CustInfo> GetBreadcrumbLevel()
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

            return breadcrumbLevel;
        }

        private IDictionary<string, object> GetBetListLinkRouteValues(ReportModel model)
           => new Dictionary<string, object>
           {
               //["CustId"] = model.CustId,
               ["CustName"] = model.Username,
               ["FromDate"] = query.FromDate,
               ["ToDate"] = query.ToDate,
               ["ReportType"] = query.ReportType,
               ["SportTypeGroup"] = query.UserSelectedSportGroupId,
               ["BetTypeGroup"] = query.UserSelectedBetTypeId,
               ["LiveIndicator"] = query.UserSelectedLiveStatusId,
               ["Type"] = GetBetListType(query.ReportType),
               ["BreadcrumbLevelJson"] = JsonHelper.ToJsonFunc(BuildBreadcrumbLevel(model))
           };

        private string BuildReportMainUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                UrlHelper.GetFeaturesAppPath(GetReportMainActionPath()),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = query.DisplayMode
                });
    }
}