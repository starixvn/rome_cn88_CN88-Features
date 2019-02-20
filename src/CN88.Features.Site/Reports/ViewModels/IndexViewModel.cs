namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constants;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
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

            SportGroupSelectionViewModel = new SportGroupSelectionViewModel(sportGroups, 0);
            BetTypeSelectionViewModel = new BetTypeSelectionViewModel(betTypes, 0);

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

        public SportGroupSelectionViewModel SportGroupSelectionViewModel { get; }

        public BetTypeSelectionViewModel BetTypeSelectionViewModel { get; }

        public IEnumerable<SelectListItem> WagerReportTypes =>
            new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = ReportResources.SettledWager,
                    Value = ReportTypeConstants.SettledWager.ToString(),
                    Selected=true
                },
                new SelectListItem
                {
                    Text = ReportResources.UnsettledWager,
                    Value = ReportTypeConstants.UnsettledWager.ToString(),
                }
            };

        public IEnumerable<SelectListItem> CancelledReportTypes =>
            new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = ReportResources.CancelledReport,
                    Value = ReportTypeConstants.CancelledReport.ToString(),
                    Selected=true
                },
                new SelectListItem
                {
                    Text = ReportResources.AbnormalBets,
                    Value = ReportTypeConstants.AbnormalBets.ToString(),
                }
            };

        public SettlementSummaryReport SettlementSummaryReport { get; }
    }
}