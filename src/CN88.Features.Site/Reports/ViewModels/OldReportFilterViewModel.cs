namespace CN88.Features.Site.Reports.ViewModels
{
    using Features.Shared.Models;
    using Shared.ViewModels.Components;

    public class OldReportFilterViewModel : ReportFilterBaseViewModel
    {
        public OldReportFilterViewModel(
            int displayMode,
            int reportType,
            bool isOwner,
            int custId,
            string custName,
            string breadcrumbLevelJson,
            UserLevel custLevel,
            SingleSelectionViewModel sportGroupSelectionViewModel,
            AccountingPeriodSelectionViewModel accountingPeriodSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel)
            : base(displayMode, reportType, isOwner, custId, custName, breadcrumbLevelJson, custLevel, sportGroupSelectionViewModel, dateRangePickerViewModel)
        {
            AccountingPeriodSelectionViewModel = accountingPeriodSelectionViewModel;
        }

        public AccountingPeriodSelectionViewModel AccountingPeriodSelectionViewModel { get; }
    }
}