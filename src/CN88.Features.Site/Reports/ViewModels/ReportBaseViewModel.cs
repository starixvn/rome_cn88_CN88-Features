namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using Constants;
    using Features.BetLists.Constants;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;

    public abstract class ReportBaseViewModel
    {
        private const string DefaultSortPropertyName = nameof(ReportModel.Username);

        protected readonly ReportBaseQuery query;

        private readonly IDictionary<int, int> BetListTypes = new Dictionary<int, int>
        {
            [ReportTypeConstants.SettledWager] = BetListTypeConstant.Settled,
            [ReportTypeConstants.UnsettledWager] = BetListTypeConstant.Unsettled,
            [ReportTypeConstants.CancelledReport] = BetListTypeConstant.Cancelled,
            [ReportTypeConstants.AbnormalBets] = BetListTypeConstant.Abnormal,
        };

        protected ReportBaseViewModel(
            ReportBaseQuery query,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal)
        {
            this.query = query;

            ReportData = reportData;
            GrandTotal = grandTotal;
            Divisor = (query.ActorLevel.IsHeadline()) ? 100 : 1;
            TotalStockPercent = query.ReportType == ReportTypeConstants.SettledWager ? 100 : 0;
            GetSortedProperty(query);
        }

        public bool IsNotOwner => !query.IsOwner;

        public UserLevel ReportLevel => query.CustLevel;

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsSettlement => query.IsSettlement.Value;

        public int Divisor { get; }

        public int TotalStockPercent { get; }

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

        protected abstract bool IsOldReport();

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
                    ["PageSize"] = query.PageSize,
                    ["IsSettlement"] = query.IsSettlement,
                    ["CustLevelId"] = ReportLevel.GetDirectDownLineLevel().Value,
                    ["IsDescendingSort"] = IsDescendingSort,
                    ["SortPropertyName"] = SortPropertyName
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
        {
            var routeValues = GetDownlineLinkRouteValues(model);

            return new Dictionary<string, object>(routeValues)
            {
                ["Type"] = GetBetListType(query.ReportType),
                ["DisplayMode"] = query.DisplayMode,
                ["ReportMainActionPath"] = GetReportMainActionPath(),
                ["IsOldReport"] = IsOldReport(),
            };
        }

        private string BuildReportMainUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                UrlHelper.GetFeaturesAppPath(GetReportMainActionPath()),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = query.DisplayMode
                });

        private int GetBetListType(int reportType)
            => BetListTypes[reportType];

        private void GetSortedProperty(ReportBaseQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.SortPropertyName))
            {
                SortPropertyName = DefaultSortPropertyName;
                IsDescendingSort = false;
            }
            else
            {
                var sortProperties = ReportSortPropertyConstants.SortPropertiesByLevel[query.CustLevel];
                if (sortProperties.Contains(query.SortPropertyName, StringComparer.OrdinalIgnoreCase))
                {
                    SortPropertyName = query.SortPropertyName;
                    IsDescendingSort = query.IsDescendingSort;
                }
                else
                {
                    SortPropertyName = DefaultSortPropertyName;
                    IsDescendingSort = false;
                }
            }

        }
    }
}