namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using CN88.Features.Reports.Models;

    public class AccountingSheetViewModel
    {
        public AccountingSheetViewModel(
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            AccountingPeriod currentPeriod,
            AccountingPeriod lastPeriod)
        {
            AccountingPeriodGroups = accountingPeriodGroups;
            CurrentPeriod = currentPeriod;
            LastPeriod = lastPeriod;
        }

        public AccountingPeriod CurrentPeriod { get; }

        public AccountingPeriod LastPeriod { get; }

        public IEnumerable<AccountingPeriodGroup> AccountingPeriodGroups { get; }
    }
}