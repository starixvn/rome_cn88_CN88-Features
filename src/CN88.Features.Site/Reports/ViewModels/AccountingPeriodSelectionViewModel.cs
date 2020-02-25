namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using Features.Reports.Models;

    public class AccountingPeriodSelectionViewModel
    {
        public AccountingPeriodSelectionViewModel(
            IEnumerable<AccountingPeriod> periods,
            string userSelectedPeriodId)
        {
            Periods = periods;
            UserSelectedPeriodId = userSelectedPeriodId;
        }

        public IEnumerable<AccountingPeriod> Periods { get; }

        public string UserSelectedPeriodId { get; }
    }
}