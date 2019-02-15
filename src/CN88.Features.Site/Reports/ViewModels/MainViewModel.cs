namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Features.Reports.Models;
    using Features.Shared.Models;

    public class MainViewModel
    {
        public MainViewModel(
            int reportType,
            DateTime fromDate,
            DateTime toDate,
            IEnumerable<Sport> sports,
            IEnumerable<BetType> betTypes,
            IEnumerable<AccountingGroup> accountingGroups)
        {
            ReportType = reportType;
            FromDate = fromDate;
            ToDate = toDate;
            Sports = sports;
            BetTypes = betTypes;
            AccountingGroups = accountingGroups;
        }

        public int ReportType { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public IEnumerable<Sport> Sports { get; }

        public IEnumerable<BetType> BetTypes { get; }

        public IEnumerable<AccountingGroup> AccountingGroups { get; }

        public int MaxPeriodId => AccountingGroups.Max(g => g.Periods.Count);
    }
}