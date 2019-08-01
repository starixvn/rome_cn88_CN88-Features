namespace CN88.Features.Site.Reports.ViewModels
{
    using CN88.Features.Reports.Models;
    using Features.Shared.Models;
    using Shared.ViewModels.Components;

    public class ReportFilterBaseViewModel
    {
        public ReportFilterBaseViewModel(
            int displayMode,
            int reportType,
            bool isOwner,
            int custId,
            string custName,
            string breadcrumbLevelJson,
            UserLevel custLevel,
            SingleSelectionViewModel sportGroupSelectionViewModel,
            DateRangePickerViewModel dateRangePickerViewModel,
            bool isMobile,
            bool isShowPermissionOnDownlinesMessage,
            AssignedDownlinesCount assignedDownlinesCount)
        {
            DisplayMode = displayMode;
            ReportType = reportType;
            IsOwner = isOwner;
            CustId = custId;
            CustName = custName;
            CustLevel = custLevel;
            BreadcrumbLevelJson = breadcrumbLevelJson;
            SportGroupSelectionViewModel = sportGroupSelectionViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
            IsMobile = isMobile;
            IsShowPermissionOnDownlinesMessage = isShowPermissionOnDownlinesMessage;
            AssignedDownlinesCount = assignedDownlinesCount;
        }

        public int DisplayMode { get; }

        public int ReportType { get; }

        public bool IsOwner { get; }

        public int CustId { get; }

        public string CustName { get; }

        public UserLevel CustLevel { get; }

        public string BreadcrumbLevelJson { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }

        public bool IsMobile { get; }

        public bool IsShowPermissionOnDownlinesMessage { get; }

        public AssignedDownlinesCount AssignedDownlinesCount { get; }
    }
}