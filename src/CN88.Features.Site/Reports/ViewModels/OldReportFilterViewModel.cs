namespace CN88.Features.Site.Reports.ViewModels
{
    using Features.Shared.Models;
    using Shared.ViewModels.Components;

    public class OldReportFilterViewModel : ReportFilterBaseViewModel
    {
        public OldReportFilterViewModel(
            int reportType,
            bool isOwner,
            int custId,
            string custName,
            UserLevel custLevel,
            SingleSelectionViewModel sportGroupSelectionViewModel,
            AccountingPeriodSelectionViewModel accountingPeriodSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel)
            : base(reportType, isOwner, custId, custName, custLevel, sportGroupSelectionViewModel, dateRangePickerViewModel)
        {
            AccountingPeriodSelectionViewModel = accountingPeriodSelectionViewModel;
        }

        public AccountingPeriodSelectionViewModel AccountingPeriodSelectionViewModel { get; }
    }
}