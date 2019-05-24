namespace CN88.Features.Site.BetLists.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Web.Helpers;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Constants;
    using Features.BetLists.Models;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Configurations;
    using Features.Shared.Constants;
    using Features.Shared.Models;
    using Reports.Constants;
    using Reports.ViewModels;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class BetListOfReportViewModel : BetListTableViewModel
    {
        private readonly IDictionary<int, int> ReportTypes = new Dictionary<int, int>
        {
            [BetListTypeConstant.Settled] = ReportTypeConstants.SettledWager,
            [BetListTypeConstant.OldSettled] = ReportTypeConstants.SettledWager,
            [BetListTypeConstant.Unsettled] = ReportTypeConstants.UnsettledWager,
            [BetListTypeConstant.Cancelled] = ReportTypeConstants.CancelledReport,
            [BetListTypeConstant.OldCancelled] = ReportTypeConstants.CancelledReport,
            [BetListTypeConstant.Abnormal] = ReportTypeConstants.AbnormalBets,
            [BetListTypeConstant.OldAbnormal] = ReportTypeConstants.AbnormalBets,
        };

        protected readonly BetListOfReportQuery message;

        public BetListOfReportViewModel(
            BetListOfReportQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            bool isMobile = false,
            string[] excludingTicketStatus = null)
        {
            Divisor = (message.ActorLevel.IsHeadline()) ? 100 : 1;

            this.message = message;
            CustLevel = message.ActorLevel;
            DisplayVNIP = message.DisplayVNIP;

            NoOfColumns = CustLevel.IsSuperLevel() || CustLevel.IsHeadline() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, Divisor, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;

            FromDate = message.FromDate;
            ToDate = message.ToDate;
            SportTypeGroup = message.UserSelectedSportGroupId;
            BetTypeGroup = message.UserSelectedBetTypeId;
            CustId = message.CustId;
            Type = message.Type;
            IsOwner = message.IsOwner;

            IsMobile = isMobile;

            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;

            ReportType = ReportTypes[message.Type];
            ReportTitle = ReportResources.ResourceManager.GetString(ReportTypeConstants.ReportTypes[ReportType]);
        }

        public BetListOfReportViewModel(
            BetListOfReportQuery message,
            BetListData betListData,
            ITicketHelper ticketHelper,
            IBetListCN88HtmlBuilder cn88Builder,
            IBetListHTMLBuilder builder,
            IEnumerable<SelectListItemExt> sportGroups,
            IEnumerable<SelectListItemExt> betTypes,
            AccountingPeriod currentPeriod,
            AccountingPeriod lastPeriod,
            IEnumerable<SelectListItemExt> allBetTypes,
            IEnumerable<AccountingPeriod> oldAccountingPeriods,
            bool isMobile = false,
            string[] excludingTicketStatus = null) : this(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            DisplayMode = message.DisplayMode;
            ReportMainActionPath = message.ReportMainActionPath;
            IsOldReport = message.IsOldReport;

            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.UserSelectedSportGroupId);

            var dateRangePickerViewModel = message.IsOldReport
                ? new DateRangePickerViewModel(oldAccountingPeriods.Last().FromDate, oldAccountingPeriods.First().ToDate, message.FromDate, message.ToDate)
                : new DateRangePickerViewModel(lastPeriod.FromDate, DateTime.MaxValue, message.FromDate, message.ToDate);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, message.UserSelectedBetTypeId);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItemExt
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == message.UserSelectedLiveStatusId
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, message.UserSelectedLiveStatusId);

            var allBetTypesSelectionViewModel = new SingleSelectionViewModel(allBetTypes, 0);

            ReportFilterViewModel = new ReportFilterViewModel(
                DisplayMode,
                ReportType,
                message.IsOwner,
                message.CustId,
                message.CustName,
                JsonHelper.ToJsonFunc(message.BreadcrumbLevel),
                message.CustLevel,
                sportGroupSelectionViewModel,
                dateRangePickerViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel,
                allBetTypesSelectionViewModel);

            PageIndex = message.PageIndex;
            PageSize = message.PageSize;
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, betListData.Tickets.Count());

            var reportActionName = DisplayMode == ReportDisplayModeConstants.Wager ? "Wager" : "Legacy";
            var reportActionController = message.IsOldReport ? "OldReports" : "Reports";

            var reportPath = $"~/{reportActionController}/{reportActionName}";

            BreadcrumbParamByLevel = BuildBreadcrumbParamByLevel(reportPath, CustLevel);
        }

        public bool IsOldReport { get; set; }

        public bool IsOwner { get; set; }

        public string OldMainPath => MicroSites.OldMainUrl("~");

        public string ResultsPath => AppPathHelper.GetAppPath(AppSettings.ResultAppPath, "~");

        public string TotalBetsForecastPath => AppPathHelper.GetAppPath(AppSettings.TotalBetsForecastAppPath, "~");

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int SportTypeGroup { get; }

        public int BetTypeGroup { get; }

        public int LiveIndicator { get; }

        public int Type { get; }

        public int CustId { get; }

        public int ViewerIdOrViewerAdminId { get; }

        public byte ViewerLevelId { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public string ReportTitle { get; private set; }

        public int DisplayMode { get; private set; }

        public string ReportMainActionPath { get; private set; }

        public ReportFilterViewModel ReportFilterViewModel { get; private set; }

        public int ReportType { get; private set; }

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel
            => new BreadcrumbLevelViewModel(BuildReportMainUrl(), GetBreadcrumbLevel());

        public PagerViewModel PagerViewModel { get; private set; }

        public IEnumerable<string> BreadcrumbParamByLevel { get; private set; }

        private string BuildReportMainUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                UrlHelper.GetFeaturesAppPath(ReportMainActionPath),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = DisplayMode
                });

        private IEnumerable<CustInfo> GetBreadcrumbLevel()
        {
            var breadcrumbLevel = new List<CustInfo>();

            if (message.BreadcrumbLevel?.Any() == true)
            {
                foreach (var custInfo in message.BreadcrumbLevel)
                {
                    breadcrumbLevel.Add(custInfo);
                }
            }

            return breadcrumbLevel;
        }

        public BreadcrumbItem BuildBackLink(string requestUrl)
            => BreadcrumbLevelViewModel.BuildBreadcrumbLinks(requestUrl, message.IsSettlement, BreadcrumbParamByLevel).LastOrDefault()
                ?? new BreadcrumbItem("Not Found", "#");

        public static IEnumerable<string> BuildBreadcrumbParamByLevel(string reportPath, UserLevel custLevel)
        {
            var breadcrumbCount = custLevel.IsHeadline()
                ? 4
                : custLevel.IsMemberLevel()
                ? 0
                : custLevel.Value;

            return Enumerable.Repeat(reportPath, breadcrumbCount);
        }
    }
}
