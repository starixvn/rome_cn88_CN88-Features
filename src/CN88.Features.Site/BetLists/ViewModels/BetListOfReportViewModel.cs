namespace CN88.Features.Site.BetLists.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
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
    using Reports.ViewModels;
    using Requests;
    using Shared.ViewModels.Components;

    public class BetListOfReportViewModel
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
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            AccountingPeriod currentPeriod,
            AccountingPeriod lastPeriod,
            string[] excludingTicketStatus = null)
        {
            this.message = message;
            CustLevel = message.CustLevel;
            DisplayVNIP = message.DisplayVNIP;

            NoOfColumns = CustLevel.IsSuperLevel() ? 11 : CustLevel.IsMasterLevel() ? 9 : 8;

            BetListData = betListData;
            BetListTotal = new BetListTotal(betListData, excludingTicketStatus);
            TicketHelper = ticketHelper;
            CN88Builder = cn88Builder;
            Builder = builder;

            FromDate = message.FromDate;
            ToDate = message.ToDate;
            SportTypeGroup = message.SportTypeGroup;
            BetTypeGroup = message.BetTypeGroup;
            CustId = message.CustId;
            Type = message.Type;

            ViewerIdOrViewerAdminId = message.ViewerIdOrViewerAdminId;
            ViewerLevelId = message.ViewerLevelId;

            BuildReportFilterViewModel(message, sportGroups, betTypes, currentPeriod, lastPeriod);
        }

        public UserLevel CustLevel { get; }

        public int NoOfColumns { get; }

        public BetListData BetListData { get; }

        public BetListTotal BetListTotal { get; }

        public ITicketHelper TicketHelper { get; }

        public IBetListHTMLBuilder Builder { get; }

        public IBetListCN88HtmlBuilder CN88Builder { get; }

        public bool DisplayVNIP { get; }

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

        public string ReportTitle { get; private set; }

        public int DisplayMode { get; private set; }

        public string ReportMainActionPath { get; private set; }

        public ReportFilterViewModel ReportFilterViewModel { get; private set; }

        public int ReportType { get; private set; }

        public decimal CompanyPositionTaking(TicketDto ticket)
        {
            return 100 - ticket.AgentPositionTaking - ticket.MasterPositionTaking - ticket.SuperPositionTaking;
        }

        public BreadcrumbLevelViewModel BreadcrumbLevelViewModel
            => new BreadcrumbLevelViewModel(BuildReportMainUrl(), GetBreadcrumbLevel());

        private string BuildReportMainUrl()
            => Shared.Helpers.UrlHelper.AddQueryStringParamsFunc(
                Shared.Helpers.UrlHelper.GetFeaturesAppPath(ReportMainActionPath),
                new Dictionary<string, object>
                {
                    ["DisplayMode"] = DisplayMode
                });

        private IEnumerable<CustInfo> GetBreadcrumbLevel()
        {
            var breadcrumbLevel = new List<CustInfo>();

            if (message.BreadcrumbLevel != null
                && message.BreadcrumbLevel.Any())
            {
                foreach (var custInfo in message.BreadcrumbLevel)
                {
                    if (custInfo.CustLevelId == message.CustLevelId)
                    {
                        break;
                    }

                    breadcrumbLevel.Add(custInfo);
                }
            }

            return breadcrumbLevel;
        }

        private void BuildReportFilterViewModel(
            BetListOfReportQuery message,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            AccountingPeriod currentPeriod,
            AccountingPeriod lastPeriod)
        {
            ReportType = ReportTypes[message.Type];
            ReportTitle = ReportResources.ResourceManager.GetString(ReportTypeConstants.ReportTypes[ReportType]);
            DisplayMode = 1;
            ReportMainActionPath = "~/Reports/Index";

            var sportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, message.SportTypeGroup);
            var dateRangePickerViewModel = new DateRangePickerViewModel(lastPeriod.FromDate, DateTime.MaxValue, message.FromDate, message.ToDate);
            var betTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, message.BetTypeGroup);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                    Selected = s.Key == message.LiveIndicator
                });
            var liveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, message.LiveIndicator);

            ReportFilterViewModel = new ReportFilterViewModel(
                ReportType,
                message.IsOwner,
                message.CustId,
                message.CustName,
                message.CustLevel,
                sportGroupSelectionViewModel,
                dateRangePickerViewModel,
                betTypeSelectionViewModel,
                liveStatusSelectionViewModel);
        }
    }
}