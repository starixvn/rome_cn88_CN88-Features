namespace CN88.Features.Site.Reports.ViewModels
{
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
            DateRangePickerViewModel dateRangePickerViewModel)
        {
            DisplayMode = displayMode;
            ReportType = reportType;
            IsOwner = isOwner;
            CustId = custId;
            CustName = custName;
            BreadcrumbLevelJson = breadcrumbLevelJson;
            CustLevel = custLevel;
            SportGroupSelectionViewModel = sportGroupSelectionViewModel;
            DateRangePickerViewModel = dateRangePickerViewModel;
        }

        public int DisplayMode { get; }

        public int ReportType { get; }

        public bool IsOwner { get; }

        public int CustId { get; }

        public string CustName { get; }

        public string BreadcrumbLevelJson { get; }

        public UserLevel CustLevel { get; }

        public SingleSelectionViewModel SportGroupSelectionViewModel { get; }

        public DateRangePickerViewModel DateRangePickerViewModel { get; }
    }
}