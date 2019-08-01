namespace CN88.Features.Site.Reports.ViewModels
{
    using CN88.Features.Reports.Models;
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
            DateRangePickerViewModel dateRangePickerViewModel,
            bool isMobile = false,
            bool isShowPermissionOnDownlinesMessage = false,
            AssignedDownlinesCount assignedDownlinesCount = null)
            : base(
                  displayMode,
                  reportType,
                  isOwner,
                  custId,
                  custName,
                  breadcrumbLevelJson,
                  custLevel,
                  sportGroupSelectionViewModel,
                  dateRangePickerViewModel,
                  isMobile,
                  isShowPermissionOnDownlinesMessage,
                  assignedDownlinesCount)
        {
            AccountingPeriodSelectionViewModel = accountingPeriodSelectionViewModel;
        }

        public AccountingPeriodSelectionViewModel AccountingPeriodSelectionViewModel { get; }
    }
}