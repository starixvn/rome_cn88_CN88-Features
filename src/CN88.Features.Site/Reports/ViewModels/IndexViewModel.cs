namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Constants;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
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
            FromDate = query.FromDate;
            ToDate = query.ToDate;
            AccountingPeriodGroups = accountingPeriodGroups;
            CurrentPeriod = currentPeriod;

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

        public string ActionName { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<AccountingPeriodGroup> AccountingPeriodGroups { get; }

        public AccountingPeriod CurrentPeriod { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

        public IEnumerable<SelectListItem> WagerReportTypes => ReportTypeConstants.WagerReportTypes
            .Select(t => new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(t.Value),
                Value = t.Key.ToString(),
            });

        public IEnumerable<SelectListItem> CancelledReportTypes => ReportTypeConstants.CancelledReportTypes
            .Select(t => new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(t.Value),
                Value = t.Key.ToString(),
            });

        public SettlementSummaryReport SettlementSummaryReport { get; }
    }
}