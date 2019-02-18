namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Features.Shared.Models;

    public class MainViewModel
    {
        public MainViewModel(
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<Sport> sports,
            IEnumerable<BetType> betTypes,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Sports = sports;
            BetTypes = betTypes;
            AccountingPeriodGroups = accountingPeriodGroups;
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

        public IEnumerable<Sport> Sports { get; }

        public IEnumerable<BetType> BetTypes { get; }

        public IEnumerable<AccountingPeriodGroup> AccountingPeriodGroups { get; }
    }
}