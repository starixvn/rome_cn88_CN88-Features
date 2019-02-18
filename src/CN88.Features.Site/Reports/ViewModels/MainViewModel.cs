namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Shared.ViewModels.Components;

    public class MainViewModel
    {
        public MainViewModel(
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<SelectListItem> sportGroups,
            IEnumerable<SelectListItem> betTypes,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            AccountingPeriod currentPeriod)
        {
            FromDate = fromDate;
            ToDate = toDate;
            AccountingPeriodGroups = accountingPeriodGroups;
            CurrentPeriod = currentPeriod;

            SportGroupSelectionViewModel = new SportGroupSelectionViewModel(sportGroups, 0);
            BetTypeSelectionViewModel = new BetTypeSelectionViewModel(betTypes, 0);
        }

        public IEnumerable<SelectListItem> WagerReportTypes =>
            new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = ReportResources.SettledWager,
                    Value = ReportTypeConstants.SettledWager.ToString(),
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
                },
                new SelectListItem
                {
                    Text = ReportResources.AbnormalBets,
                    Value = ReportTypeConstants.AbnormalBets.ToString(),
                }
            };

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<AccountingPeriodGroup> AccountingPeriodGroups { get; }

        public AccountingPeriod CurrentPeriod { get; }

        public SportGroupSelectionViewModel SportGroupSelectionViewModel { get; }

        public BetTypeSelectionViewModel BetTypeSelectionViewModel { get; }
    }
}