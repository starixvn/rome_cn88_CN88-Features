namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Features.Shared.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class IndexViewModel
    {
        public IndexViewModel(
            IndexQuery query,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            AccountingPeriod currentPeriod,
            SettlementSummaryReport settlementSummaryReport)
        {
            DisplayMode = query.DisplayMode;
            CustLevelId = query.ActorLevel.IsHeadline() ? UserLevel.Super.Value : query.ActorLevel.Value;
            IsOwner = query.ActorLevel.IsNotHeadline();

            AccountingPeriodGroups = accountingPeriodGroups;
            CurrentPeriod = currentPeriod;

            DateRangePickerViewModel = new DateRangePickerViewModel(currentPeriod.FromDate, currentPeriod.ToDate, query.FromDate, query.ToDate);
            SportGroupSelectionViewModel = new SingleSelectionViewModel(sportGroups, 0);
            BetTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, 0);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                });
            LiveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, LiveStatusConstants.PreMatchAndInPlay);

            ActionName = query.DisplayMode == ReportDisplayModeConstants.Wager
                ? nameof(ReportDisplayModeConstants.Wager)
                : nameof(ReportDisplayModeConstants.Legacy);

            SettlementSummaryReport = settlementSummaryReport;
        }

        public int DisplayMode { get; }

        public int CustLevelId { get; }

        public bool IsOwner { get; }

        public string ActionName { get; }

        public IEnumerable<AccountingPeriodGroup> AccountingPeriodGroups { get; }

        public AccountingPeriod CurrentPeriod { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public IEnumerable<SelectListItem> WagerReportTypes => ReportTypeConstants.WagerReportTypes
            .Select(t => new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(t.Value),
                Value = t.Key.ToString(),
                Selected = t.Key == ReportTypeConstants.SettledWager
            });

        public IEnumerable<SelectListItem> CancelledReportTypes => ReportTypeConstants.CancelledReportTypes
            .Select(t => new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(t.Value),
                Value = t.Key.ToString(),
                Selected = t.Key == ReportTypeConstants.CancelledReport
            });

        public SettlementSummaryReport SettlementSummaryReport { get; }
    }
}