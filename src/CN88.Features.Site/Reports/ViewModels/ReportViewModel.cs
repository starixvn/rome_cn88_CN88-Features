namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Web.Helpers;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Features.Shared.Models;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels.Components;

    public class ReportViewModel
    {
        private const string ReportMainActionPath = "~/Reports/Index";

        private readonly ReportQuery query;

        public ReportViewModel(
            ReportQuery query,
            IEnumerable<CustInfo> breadcrumbLevel,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            IEnumerable<WinLossModel> reportData,
            WinLossModel grandTotal,
            AccountingPeriod currentPeriod)
        {
            this.query = query;

            ReportData = reportData;
            GrandTotal = grandTotal;

            var breadcrumbLevelJson = JsonHelper.ToJsonFunc(breadcrumbLevel);
            BreadcrumbLevelViewModel = new BreadcrumbLevelViewModel(BuildReportMainUrl(), breadcrumbLevel, breadcrumbLevelJson);

            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, query.UserSelectedSportGroupId);
            var dateRangePickerViewModel = new DateRangePickerViewModel(currentPeriod.FromDate, currentPeriod.ToDate, query.FromDate, query.ToDate);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, query.UserSelectedBetTypeId);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == query.UserSelectedLiveStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, query.UserSelectedLiveStatusId);

            ReportFilterViewModel = new ReportFilterViewModel(
                query.ReportType,
                query.IsOwner,
                query.CustId,
                query.CustName,
                query.CustLevel,
                sportGroupSelectionViewModel,
                dateRangePickerViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel);
        }

        public bool IsNotOwner => !query.IsOwner;

        public UserLevel ReportLevel => query.CustLevel;

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel { get; set; }

        public IEnumerable<WinLossModel> ReportData { get; }

        public WinLossModel GrandTotal { get; }

        public ReportFilterViewModel ReportFilterViewModel { get; }

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(query.PageIndex, query.PageSize, ReportData.Count());

        public string Title => ReportResources.ResourceManager.GetString(ReportTypeConstants.ReportTypes[query.ReportType]);

        public string BuildDrillDownLink(WinLossModel model, string currentLink)
            => ReportLevel.IsMemberLevel()
            ? BuildBetListLink(model)
            : BuildDownlineLink(model, currentLink);

        private string BuildBetListLink(WinLossModel model)
            => Shared.Helpers.UrlHelper.AddQueryStringParamsFunc(
                MicroSites.BetListsUrl(),
                GetBetListLinkRouteValues(model));

        private string BuildDownlineLink(WinLossModel model, string currentLink)
        {
            var routeValues = GetDownlineLinkRouteValues(model);
            return Shared.Helpers.UrlHelper.ReplaceOrAddQueryStringParamsFunc(
                currentLink,
                new Dictionary<string, object>(routeValues)
                {
                    ["CustLevelId"] = ReportLevel.GetDirectDownLineLevel().Value,
                    ["BreadcrumbLevelJson"] = BreadcrumbLevelViewModel.BreadcrumbLevelJson
                });
        }

        private IDictionary<string, object> GetDownlineLinkRouteValues(WinLossModel model)
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
                ["UserSelectedLiveStatusId"] = query.UserSelectedLiveStatusId
            };

        private IDictionary<string, object> GetBetListLinkRouteValues(WinLossModel model)
           => new Dictionary<string, object> { };

        private string BuildReportMainUrl()
            => Shared.Helpers.UrlHelper.AddQueryStringParamsFunc(
                Shared.Helpers.UrlHelper.GetFeaturesAppPath(ReportMainActionPath),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = query.DisplayMode
                });
    }
}