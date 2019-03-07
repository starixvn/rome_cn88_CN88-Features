namespace CN88.Features.Site.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.Models;
    using Requests;
    using Shared.ViewModels.Components;

    public class OldIndexViewModel : IndexBaseViewModel
    {
        public OldIndexViewModel(
            OldIndexQuery query,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            IEnumerable<AccountingPeriod> oldAccountingPeriods,
            IEnumerable<SelectListItem> sportGroups)
            : base(query, accountingPeriodGroups, sportGroups)
        {
            var selectPeriod = oldAccountingPeriods.First();
            AccountingPeriodSelectionViewModel = new AccountingPeriodSelectionViewModel(oldAccountingPeriods, selectPeriod.PeriodId);
            DateRangePickerViewModel = new DateRangePickerViewModel(DateTime.MinValue, selectPeriod.ToDate, selectPeriod.FromDate, selectPeriod.ToDate);
        }

        public AccountingPeriodSelectionViewModel AccountingPeriodSelectionViewModel { get; }
    }
}