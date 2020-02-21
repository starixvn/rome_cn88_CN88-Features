namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CN88.Features.Shared.AppResources;
    using CN88.Web.Helpers;
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

        protected readonly ReportBaseQuery reportQuery;

        private readonly IDictionary<int, int> BetListTypes = new Dictionary<int, int>
        {
            [ReportTypeConstants.SettledWager] = BetListTypeConstant.Settled,
            [ReportTypeConstants.UnsettledWager] = BetListTypeConstant.Unsettled,
            [ReportTypeConstants.CancelledReport] = BetListTypeConstant.Cancelled,
            [ReportTypeConstants.AbnormalBets] = BetListTypeConstant.Abnormal,
        };

        protected ReportBaseViewModel(
            ReportBaseQuery reportQuery,
            IEnumerable<ReportModel> reportData,
            ReportModel grandTotal,
            bool isMobile = false)
        {
            this.reportQuery = reportQuery;

            ReportData = reportData.ToList();
            GrandTotal = grandTotal;
            Divisor = reportQuery.Divisor;
            TotalStockPercent = reportQuery.ReportType == ReportTypeConstants.SettledWager ? 100 : 0;
            IsMobile = isMobile;
            GetSortedProperty(reportQuery);
            IsOwner = reportQuery.IsOwner;
            IsOwnerText = IsOwner.ToString().ToLowerInvariant();
            IsNotOwnerText = IsNotOwner.ToString().ToLowerInvariant();
        }

        public bool IsNotOwner => !reportQuery.IsOwner;

        public string IsOwnerText { get; }

        public string IsNotOwnerText { get; }

        public bool IsOwner { get; }

        public UserLevel ReportLevel => reportQuery.CustLevel;

        public string SortPropertyName { get; set; }

        public bool IsDescendingSort { get; set; }

        public bool IsSettlement => reportQuery.IsSettlement.Value;

        public bool IsUnsettledOrCancelledReport =>
            reportQuery.ReportType == ReportTypeConstants.UnsettledWager
            || reportQuery.ReportType == ReportTypeConstants.CancelledReport
            || reportQuery.ReportType == ReportTypeConstants.AbnormalBets;

        public int Divisor { get; }

        public int TotalStockPercent { get; }

        public IEnumerable<ReportModel> ReportData { get; }

        public ReportModel GrandTotal { get; }

        public decimal ProfitMargin
            => reportQuery.CustLevel.IsSuperLevel() && GrandTotal.SuperStock != 0
                ? GrandTotal.SuperResult / GrandTotal.SuperStock * 100
                : 0;

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel
            => new BreadcrumbLevelViewModel(BuildReportMainUrl(), GetBreadcrumbLevel());

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(reportQuery.PageIndex, reportQuery.PageSize, ReportData.Count());

        public string Title => ReportResources.ResourceManager.GetString(ReportTypeConstants.ReportTypes[reportQuery.ReportType]);

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
                    ["PageSize"] = reportQuery.PageSize,
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
                ["FromDate"] = reportQuery.FromDate,
                ["ToDate"] = reportQuery.ToDate,
                ["IsOwner"] = false,
                ["IsSettlement"] = reportQuery.IsSettlement,
                ["ReportType"] = reportQuery.ReportType,
                ["UserSelectedSportGroupId"] = reportQuery.UserSelectedSportGroupId,
                ["UserSelectedBetTypeId"] = reportQuery.UserSelectedBetTypeId,
                ["UserSelectedLiveStatusId"] = reportQuery.UserSelectedLiveStatusId,
                ["BreadcrumbLevelJson"] = JsonHelper.ToJsonFunc(BuildBreadcrumbLevel(model)),
                ["UserSelectedPeriodId"] = reportQuery.UserSelectedPeriodId
            };

        public BreadcrumbItem BuildBackLink()
        {
            if (reportQuery.IsOwner
                || !BreadcrumbLevelViewModel.BreadcrumbLevel.Any())
            {
                return new BreadcrumbItem(SharedResources.Main, BreadcrumbLevelViewModel.MainLink);
            }

            var breadcrumbLinks = BreadcrumbLevelViewModel.BuildBreadcrumbLinks(
                reportQuery.CurrentUrl,
                reportQuery.IsSettlement ?? false);

            return breadcrumbLinks.Count() > 1
                    ? breadcrumbLinks.ElementAt(breadcrumbLinks.Count() - 1)
                    : breadcrumbLinks.ElementAt(0);
        }

        private IEnumerable<CustInfo> BuildBreadcrumbLevel(ReportModel model)
        {
            var breadcrumbLevel = new List<CustInfo>();

            breadcrumbLevel.AddRange(GetBreadcrumbLevel());

            breadcrumbLevel.Add(
                new CustInfo(
                    reportQuery.CustId,
                    model.Username,
                    reportQuery.CustLevelId,
                    reportQuery.IsOwner));

            return breadcrumbLevel;
        }

        private IEnumerable<CustInfo> GetBreadcrumbLevel()
        {
            var breadcrumbLevel = new List<CustInfo>();

            if (reportQuery.BreadcrumbLevel?.Any() == true)
            {
                foreach (var custInfo in reportQuery.BreadcrumbLevel)
                {
                    if (custInfo.CustLevelId == reportQuery.CustLevelId)
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
                ["Type"] = GetBetListType(reportQuery.ReportType),
                ["DisplayMode"] = reportQuery.DisplayMode,
                ["ReportMainActionPath"] = GetReportMainActionPath(),
                ["IsOldReport"] = IsOldReport(),
            };
        }

        private string BuildReportMainUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                UrlHelper.GetFeaturesAppPath(GetReportMainActionPath()),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = reportQuery.DisplayMode
                });

        private int GetBetListType(int reportType)
            => BetListTypes[reportType];

        public bool IsMobile { get; }

        public bool IsShowTotalRow()
            => ReportData.Any()
                && (!IsMobile || (IsMobile && IsNotOwner));

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
