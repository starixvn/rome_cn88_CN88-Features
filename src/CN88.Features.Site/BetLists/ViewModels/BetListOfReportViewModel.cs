namespace CN88.Features.Site.BetLists.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BetList.Core.Builders;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Features.BetLists.Constants;
    using Features.BetLists.Models;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Reports.Constants;
    using Reports.ViewModels;
    using Requests;
    using Shared.Helpers;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class BetListOfReportViewModel : BetListViewModelBase
    {
        private static readonly IDictionary<int, int> ReportTypes = new Dictionary<int, int>
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
            : base(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            this.message = message;

            FromDate = message.FromDate;
            ToDate = message.ToDate;
            SportTypeGroup = message.UserSelectedSportGroupId;
            BetTypeGroup = message.UserSelectedBetTypeId;
            Type = message.Type;
            IsOwner = message.IsOwner;

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
            AccountingPeriod lastPeriod,
            IEnumerable<SelectListItemExt> allBetTypes,
            IEnumerable<AccountingPeriod> oldAccountingPeriods,
            bool isMobile = false,
            string[] excludingTicketStatus = null)
            : this(message, betListData, ticketHelper, cn88Builder, builder, isMobile, excludingTicketStatus)
        {
            DisplayMode = message.DisplayMode;
            ReportMainActionPath = message.ReportMainActionPath;
            IsOldReport = message.IsOldReport;

            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.UserSelectedSportGroupId);
            var dateRangePickerViewModel = CreateDateRangePickerViewModel(message, lastPeriod, oldAccountingPeriods);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, message.UserSelectedBetTypeId);
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(GetLiveStatusList(message), message.UserSelectedLiveStatusId);
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
                allBetTypes);

            BreadcrumbParamByLevel = BuildBreadcrumbParamByLevel();
        }

        private static IEnumerable<SelectListItemExt> GetLiveStatusList(BetListOfReportQuery message)
            => LiveStatusConstants.LiveStatuses
                            .Select(s => new SelectListItemExt
                            {
                                Text = SharedResources.ResourceManager.GetString(s.Value),
                                Value = s.Key.ToString(),
                                Selected = s.Key == message.UserSelectedLiveStatusId
                            });

        private static DateRangePickerViewModel CreateDateRangePickerViewModel(BetListOfReportQuery message, AccountingPeriod lastPeriod, IEnumerable<AccountingPeriod> oldAccountingPeriods)
            => message.IsOldReport
                ? new DateRangePickerViewModel(oldAccountingPeriods.Last().FromDate, oldAccountingPeriods.First().ToDate, message.FromDate, message.ToDate)
                : new DateRangePickerViewModel(lastPeriod.FromDate, DateTime.MaxValue, message.FromDate, message.ToDate);

        private IEnumerable<string> BuildBreadcrumbParamByLevel()
        {
            var reportActionName = DisplayMode == ReportDisplayModeConstants.Wager ? "Wager" : "Legacy";
            var reportActionController = message.IsOldReport ? "OldReports" : "Reports";

            var reportPath = $"~/{reportActionController}/{reportActionName}";

            return CalculateBreadcrumbParamByLevel(reportPath);
        }

        public bool IsOldReport { get; set; }

        public bool IsOwner { get; set; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public bool IsSettlement { get; }

        public int LiveIndicator { get; }

        public int Type { get; }

        public string ReportTitle { get; private set; }

        public int DisplayMode { get; private set; }

        public string ReportMainActionPath { get; private set; }

        public ReportFilterViewModel ReportFilterViewModel { get; private set; }

        public int ReportType { get; private set; }

        public override string BuildBreadcrumbUrl()
            => UrlHelper.AddQueryStringParamsFunc(
                UrlHelper.GetFeaturesAppPath(ReportMainActionPath),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = DisplayMode
                });

        public override BreadcrumbItem BuildBackLink(string request)
            => BreadcrumbLevelViewModel.BuildBreadcrumbLinks(request, message.IsSettlement, BreadcrumbParamByLevel).LastOrDefault()
                ?? new BreadcrumbItem("Not Found", "#");

        protected override int GetMaxColumn() => 11;
    }
}