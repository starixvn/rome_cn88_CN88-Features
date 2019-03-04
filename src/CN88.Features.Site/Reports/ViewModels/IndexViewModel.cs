namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.AppResources;
    using Features.Shared.Constants;
    using Features.Shared.Utils;
    using Requests;
    using Shared.ViewModels.Components;

    public class IndexViewModel : IndexBaseViewModel
    {
        public IndexViewModel(
            IndexQuery query,
            SettlementSummaryReport settlementSummaryReport,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            AccountingPeriod currentPeriod,
            AccountingPeriod lastPeriod,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes)
            : base(query, accountingPeriodGroups, sportGroups)
        {
            CurrentPeriod = currentPeriod;
            LastPeriod = lastPeriod;
            SettlementSummaryReport = settlementSummaryReport;

            DateRangePickerViewModel = new DateRangePickerViewModel(
                lastPeriod.FromDate,
                DateTime.MaxValue,
                SystemTime.Today.AddDays(-1),
                SystemTime.Today.AddDays(-1));
            BetTypeSelectionViewModel = new SingleSelectionViewModel(betTypes, 0);

            var liveStatuses = LiveStatusConstants.LiveStatuses
                .Select(s => new SelectListItem
                {
                    Text = SharedResources.ResourceManager.GetString(s.Value),
                    Value = s.Key.ToString(),
                });
            LiveStatusSelectionViewModel = new SingleSelectionViewModel(liveStatuses, LiveStatusConstants.PreMatchAndInPlay);
        }

        public AccountingPeriod CurrentPeriod { get; }

        public AccountingPeriod LastPeriod { get; }

        public SingleSelectionViewModel BetTypeSelectionViewModel { get; }

        public SingleSelectionViewModel LiveStatusSelectionViewModel { get; }

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