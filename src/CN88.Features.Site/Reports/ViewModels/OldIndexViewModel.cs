namespace CN88.Features.Site.Reports.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Features.Reports.AppResources;
    using Features.Reports.Constants;
    using Features.Reports.Models;
    using Requests;
    using Shared.ViewModels;
    using Shared.ViewModels.Components;

    public class OldIndexViewModel : IndexBaseViewModel
    {
        public OldIndexViewModel(
            OldIndexQuery query,
            IEnumerable<AccountingPeriodGroup> accountingPeriodGroups,
            IEnumerable<AccountingPeriod> oldAccountingPeriods,
            IEnumerable<SelectListItemExt> sportGroups)
            : base(query, sportGroups)
        {
            var selectPeriod = oldAccountingPeriods.First();
            AccountingPeriodSelectionViewModel = new AccountingPeriodSelectionViewModel(oldAccountingPeriods, selectPeriod.PeriodId);
            DateRangePickerViewModel = new DateRangePickerViewModel(oldAccountingPeriods.Last().FromDate, selectPeriod.ToDate, selectPeriod.FromDate, selectPeriod.ToDate);
        }

        public AccountingPeriodSelectionViewModel AccountingPeriodSelectionViewModel { get; }

        public IEnumerable<SelectListItem> WagerReportTypes => new List<SelectListItem>
        {
            new SelectListItem
            {
                Text = ReportResources.ResourceManager.GetString(nameof(ReportTypeConstants.SettledWager)),
                Value = ReportTypeConstants.SettledWager.ToString(),
                Selected = true
            }
        };
    }
}